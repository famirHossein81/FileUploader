#nullable disable
using Microsoft.EntityFrameworkCore;
using FileUploader.Models;
using FileUploader.Repositories.Contracts;
namespace FileUploader.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly FileUploaderContext _context;

    public GenericRepository(FileUploaderContext context)
    {
        _context = context;
    }

    public async Task<T> GetByID(int id)
    {
        T entity = await _context.Set<T>().FindAsync(id);
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        List<T> list = await _context.Set<T>().ToListAsync();
        return list;
    }
    public async Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

    }

    public async Task Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
