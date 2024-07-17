using Microsoft.EntityFrameworkCore;

namespace FileUploader.Models;
public class FileUploaderContext : DbContext
{
    public FileUploaderContext(DbContextOptions<FileUploaderContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    public DbSet<File> files { get; set; }

    /* public DbSet<FileUploader.Models.File> files { get; set; } */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(p => p.Id);
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "a@a.com", Password = BCrypt.Net.BCrypt.HashPassword("a"), IsAdmin = false, IsVerified = true },
            new User { Id = 2, Email = "b@b.com", Password = BCrypt.Net.BCrypt.HashPassword("b"), IsAdmin = false, IsVerified = true }
        );
        modelBuilder.Entity<File>().HasKey(p => p.Id);
        modelBuilder.Entity<User>().HasMany(u => u.Files).WithOne(f => f.User).HasForeignKey(f => f.UserId);
        base.OnModelCreating(modelBuilder);
    }
}