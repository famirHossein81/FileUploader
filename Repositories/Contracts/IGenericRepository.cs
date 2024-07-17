namespace FileUploader.Repositories.Contracts;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByID(int id);
    Task<IEnumerable<T>> GetAll();
    Task Update(T entity);
    Task AddAsync(T entity);
    Task Remove(T entity);
}