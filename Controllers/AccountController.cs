using Microsoft.AspNetCore.Mvc;
using FileUploader.Models.Dt;
using FileUploader.Models;
using FileUploader.Repositories.Contracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FileUploader.Services;
using static FileUploader.Utils.Tools;

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

    public async Task<IActionResult> Validate(string token)
    {
        ViewBag.IsActive = await _userRepository.ActiveUser(token);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(LoginViewDto login)
    {
        if (!ModelState.IsValid)
            return View();

        User user = await _userRepository.GetByEmailAsync(login.ForgotPasswordModel.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "This user does not exist.");
            ViewBag.ErrorMessage = "This user does not exist.";
            return View("UserNotFound");
        }
        ForgotPasswordDto fpd = new()
        {
            Email = user.Email,
            ResetLink = Url.UrlGenerator("ResetPassword", "Account", new { token = user.VerificationToken })
        };

        MailRequest resetEmail = new()
        {
            ToEmail = login.ForgotPasswordModel.Email,
            Body = await _viewRenderService.RenderToStringAsync("_ForgotPassword", fpd),
            Subject = "ResetPassword"
        };
        await _emailSender.SendEmailAsync(resetEmail);

        ViewBag.IsSuccess = true;
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token)
    {
        User user = await _userRepository.GetUserByActiveCode(token);
        if (user == null) return NotFound();
        return View(new ResetPasswordDto { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
    {
        Console.WriteLine("Salam RestPassword");
        //if (!ModelState.IsValid) return View(resetPassword);]
        Console.WriteLine(resetPassword.Token);
        User user = await _userRepository.GetUserByActiveCode(resetPassword.Token);
        
        if (user == null) return NotFound();

        string hashNewPassword = BCrypt.Net.BCrypt.HashPassword(resetPassword.Password);
        user.Password = hashNewPassword;
        user.VerificationToken = TokenGenerator();
        await _userRepository.Update(user);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

}
