#nullable disable
using System.ComponentModel.DataAnnotations;
namespace FileUploader.Models.Dt;


public class ForgotPasswordDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string ResetLink { get; set; }
}