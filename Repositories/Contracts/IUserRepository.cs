#nullable disable
using FileUploader.Models;
namespace FileUploader.Repositories.Contracts;


public interface IUserRepository : IGenericRepository<User>
{

    Task<User> GetUserByIDAsync(int id);
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<bool> ActiveUser(string token);
    Task<bool> IsExistedByEmail(string email);
    Task<User> GetUserByActiveCode(string activeCode);
    Task AddFileToUserAsync(string email, Models.File file);

    Task<bool> HasFile(string email, string fileName);

    Task ChangePasswordAsync(string email, string password);

}