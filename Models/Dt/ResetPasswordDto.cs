#nullable disable
using System.ComponentModel.DataAnnotations;
namespace FileUploader.Models.Dt;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Token is not valid")]
    public string Token { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one number.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Password and confirmation are not the same.")]
    public string ConfirmPassword { get; set; }
}