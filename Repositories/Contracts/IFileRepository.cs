using FileUploader.Models;
namespace FileUploader.Repositories.Contracts;

public interface IFileRepository : IGenericRepository<Models.File>
{
    Task<IEnumerable<Models.File>> GetAllFiles();
    Task<Models.File> GetFileByID(int id);
    Task<Models.File> GetByName(string name);
    Task<IEnumerable<Models.File>> GetByType(string type);
    Task<IEnumerable<Models.File>> GetByUploadDate(DateTime date);
    Task<User> GetByUser(string email);
    Task<User> GetBySearch(string email, string search);
    Task RemoveAsync(string fileName, string email);
    Task<string> GetNameByToken(string token);


}