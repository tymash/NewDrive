using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

public abstract class BaseRepository<TId, T> : IRepository<TId, T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public virtual async Task DeleteByIdAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        _dbSet.Remove(entity ?? throw new KeyNotFoundException());
        await _context.SaveChangesAsync();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }
}