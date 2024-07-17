using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FileUploader.Models;
using FileUploader.Repositories;
using FileUploader.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using FileUploader.Models.Dt;
using FileUploader.Services;
using static FileUploader.Utils.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FileUPloader.Models;


namespace FileUploader.Controllers;

[Authorize]
public class HomeController : Controller
{
    
    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IAws3Services _awsS3Services;


    public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IFileRepository fileRepository, IAws3Services aws3Services)
    {
        _logger = logger;
        _userRepository = userRepository;
        _fileRepository = fileRepository;
        _awsS3Services = aws3Services;
    }

    public async Task<IActionResult> Index([FromQuery] string? search)
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToAction("Login", "Account");

        var viewModel = new HomeViewDto
        {
            EditInfoModel = new EditInfoDto()
        };


        if (!string.IsNullOrEmpty(search))
        {
            var userWithSearh = await _fileRepository.GetBySearch(User.Identity.Name!, search);
            viewModel.FileShowModel = new FileShowDto
            {
                Files = userWithSearh.Files
            };
        }
        else
        {
            var user = await _fileRepository.GetByUser(User.Identity.Name!);
            viewModel.FileShowModel = new FileShowDto
            {
                Files = user.Files
            };

        }
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (await _userRepository.HasFile(User.Identity!.Name!, file.FileName))
        {
            Console.WriteLine("File is already existed");
            return RedirectToAction("Index", "Home");
        }
        bool result = await _awsS3Services.UploadFile(file, User.Identity!.Name!);
        var fileEntity = new Models.File
        {
            Name = file.FileName,
            ContentType = file.ContentType,
            UploadDate = DateTime.UtcNow,
            Size = file.Length
        };
        await _userRepository.AddFileToUserAsync(User.Identity!.Name!, fileEntity);

        return RedirectToAction("Index", "Home");
    }




    [HttpGet("[action]/{fileName}")]
    public async Task<IActionResult> Download(string fileName)
    {
        if (await _userRepository.HasFile(User.Identity!.Name, fileName))
        {
            string url = await _awsS3Services.GeneratePreSignedURLAsync(fileName, User.Identity!.Name!);
            return Redirect(url);
        }
        return NotFound();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    
    [HttpGet("[action]/{fileName}")]
    public async Task<IActionResult> Remove(string fileName)
    {
        if (await _userRepository.HasFile(User.Identity!.Name, fileName))
        {
            await _fileRepository.RemoveAsync(fileName, User.Identity!.Name!);
            await _awsS3Services.Remove(fileName, User.Identity!.Name!);
            return RedirectToAction("Index", "Home");
        }
        return NotFound();
    }

    [HttpGet("[action]/{email}/{token}")]
    public async Task<IActionResult> Share(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return NotFound();

        string fileName = await _fileRepository.GetNameByToken(token);

        if (string.IsNullOrEmpty(fileName))
            return NotFound();

        string url = await _awsS3Services.GeneratePreSignedURLAsync(fileName, email);

        return Redirect(url);
    }
}
