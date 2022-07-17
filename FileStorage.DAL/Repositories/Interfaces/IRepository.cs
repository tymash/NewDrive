namespace FileStorage.DAL.Repositories.Interfaces;

public interface IRepository<TId, TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(TId id);
    Task AddAsync(TEntity entity);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(TId id);
    void Update(TEntity entity);
}