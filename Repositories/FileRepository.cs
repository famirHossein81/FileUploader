#nullable disable
using Microsoft.EntityFrameworkCore;
using FileUploader.Repositories.Contracts;
using FileUploader.Models;
namespace FileUploader.Repositories;

public class FileRespository : GenericRepository<Models.File>, IFileRepository
{

    private readonly FileUploaderContext _context;
    public FileRespository(FileUploaderContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Models.File> GetByName(string name)
    {
        Models.File file = await _context.Set<Models.File>().FindAsync(name);
        return file;
    }
    public async Task<IEnumerable<Models.File>> GetAllFiles()
    {
        List<Models.File> files = await _context.Set<Models.File>().ToListAsync();
        return files;
    }

    public async Task<Models.File> GetFileByID(int id)
    {
        Models.File file = await _context.Set<Models.File>().FindAsync(id);
        return file;
    }

    public void UpdateFile(Models.File file)
    {
        _context.Entry(file).State = EntityState.Modified;
    }

    public void AddFile(Models.File file)
    {
        _context.Set<Models.File>().Add(file);
        _context.SaveChangesAsync();
    }

    public void RemoveFile(Models.File file)
    {
        _context.Set<Models.File>().Remove(file);
        _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Models.File>> GetByType(string type)
    {
        List<Models.File> files = await _context.Set<Models.File>().Where(x => x.ContentType == type).ToListAsync();
        return files;
    }

    public Task<IEnumerable<Models.File>> GetByUploadDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetByUser(string email)
    {
        var userWithFiles = await _context.users
            .Include(u => u.Files)
            .FirstOrDefaultAsync(u => u.Email == email);
        return userWithFiles;
    }

    public async Task<User> GetBySearch(string email, string search)
    {
        var userWithFiles = await _context.users
        .Include(u => u.Files.Where(f => f.Name.ToLower().Contains(search.ToLower())))
        .FirstOrDefaultAsync(u => u.Email == email);
        return userWithFiles;
    }

    public async Task RemoveAsync(string fileName, string email)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        var file = await _context.files.FirstOrDefaultAsync(f => f.Name == fileName && f.UserId == user.Id);
        RemoveFile(file);
    }

    public async Task<string> GetNameByToken(string token)
    {
        string fileName = await _context.files.Where(f => f.Token == token).Select(f => f.Name).FirstOrDefaultAsync();
        return fileName;
    }
}