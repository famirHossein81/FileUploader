#nullable disable
using System.ComponentModel.DataAnnotations;
using static FileUploader.Utils.Tools;
namespace FileUploader.Models;


public class File
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public DateTime UploadDate { get; set; }
    public long Size { get; set; }
    public string Token { get; set; } = TokenGenerator();
    public int UserId { get; set; }
    public User User { get; set; }

}