#nullable disable
using System.ComponentModel.DataAnnotations;
namespace FileUploader.Models.Dt;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string VerificationToken { get; set; }

}