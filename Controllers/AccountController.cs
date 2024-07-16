using Microsoft.AspNetCore.Mvc;
using FileUploader.Models.Dt;
using FileUploader.Models;
using FileUploader.Repositories.Contracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileUploader.Services;
using static FileUploader.Utils.Tools;
using FileUPloader.Controllers;

namespace FileUploader.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<HomeController> _logger;
    private readonly IViewRenderService _viewRenderService;
    private readonly IEmailSender _emailSender;

    public AccountController(ILogger<HomeController> logger, IUserRepository userRepository, IViewRenderService viewRenderService, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _logger = logger;
        _viewRenderService = viewRenderService;
        _emailSender = emailSender;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        var viewModel = new LoginViewDto
        {
            UserModel = new UserDto(),
            ForgotPasswordModel = new ForgotPasswordDto()
        };

        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        return View(model: viewModel);
    }
}
