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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
