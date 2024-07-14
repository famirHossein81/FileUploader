#nullable disable
using static FileUploader.Utils.Tools;
using FileUploader.Services;
namespace FileUploader.Models;


public class User
{
    public int Id { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsVerified { get; set; } = false;

    public string VerificationToken { get; set; } = TokenGenerator();
    
    public ICollection<File> Files { get; set; } = new List<File>();

}