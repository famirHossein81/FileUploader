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

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewDto login)
    {

        if (!ModelState.IsValid) return View(model: login);


        User user = await _userRepository.GetByEmailAsync(login.UserModel.Email);

        if (user == null)
        {
            ModelState.AddModelError("Email", "There is no user with this credentials");
            return View(model: login);
        }

        if (!user.IsVerified)
        {
            ModelState.AddModelError("Email", "You should confirm your account.");
            return View(model: login);
        }

        if (BCrypt.Net.BCrypt.Verify(login.UserModel.Password, user.Password))
        {
            ViewData["Message"] = "Login Success";
            var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = true };
            await HttpContext.SignInAsync(principal, properties);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewData["Message"] = "Incorrect Credentials";
            return View(login);
        }

    }

     [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return View(model: registerDto);

        if (await _userRepository.IsExistedByEmail(registerDto.Email))
        {
            return Content("This Email is existed");
        }
        var user = new User
        {
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),

        };

        await _userRepository.AddAsync(user);
        SuccessfullRegisterDto srd = new()
        {
            Email = user.Email,
            VerificationUrl = Url.UrlGenerator("Validate", "Account", new { token = user.VerificationToken })
        };

        MailRequest verificationEmail = new()
        {
            ToEmail = registerDto.Email,
            Body = await _viewRenderService.RenderToStringAsync("_ActiveEmail", srd),
            Subject = "Activation"
        };
        await _emailSender.SendEmailAsync(verificationEmail);
        return RedirectToAction(nameof(Index));
    }

}
