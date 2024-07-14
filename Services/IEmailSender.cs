
using System.Threading.Tasks;
using FileUploader.Models;
namespace FileUploader.Services;



public interface IEmailSender {
    Task SendEmailAsync(MailRequest mailRequest);
}