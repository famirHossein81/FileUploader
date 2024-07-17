#nullable disable
using Microsoft.EntityFrameworkCore;
using FileUploader.Repositories.Contracts;
using FileUploader.Models;
using FileUploader.Services;
using static FileUploader.Utils.Tools;
namespace FileUploader.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{

    private readonly FileUploaderContext _context;

    public UserRepository(FileUploaderContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        List<User> users = await _context.Set<User>().ToListAsync();
        return users;
    }

    public async Task<User> GetUserByIDAsync(int id)
    {
        User user = await _context.Set<User>().FindAsync(id);
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        User user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        user.VerificationToken = token;
        await Update(user);
        return token;
    }

    public async Task<bool> ActiveUser(string token)
    {
        User user = await _context.users.SingleOrDefaultAsync(e => e.VerificationToken == token);
        if (user == null || user.IsVerified) return false;
        user.IsVerified = true;
        user.VerificationToken = TokenGenerator();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsExistedByEmail(string email)
    {
        User user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        return user != null;

    }

    public async Task<User> GetUserByActiveCode(string activeCode)
    {
        User user = await _context.users.FirstOrDefaultAsync(u => u.VerificationToken == activeCode);
        return user;
    }

    public async Task AddFileToUserAsync(string email, Models.File file)
    {
        var user = await GetByEmailAsync(email);
        if (user != null)
        {
            user.Files.Add(file);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasFile(string email, string fileName)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        return await _context.files.AnyAsync(f => f.UserId == user.Id && f.Name == fileName);
    }


    public async Task ChangePasswordAsync(string email, string password)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        _context.users.Update(user);
        await _context.SaveChangesAsync();

    }

}