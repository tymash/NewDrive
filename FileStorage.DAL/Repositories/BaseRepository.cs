using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

/// <summary>

/// The base repository class

/// </summary>

/// <seealso cref="IRepository{TId, T}"/>

public abstract class BaseRepository<TId, T> : IRepository<TId, T> where T : class
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly DbContext _context;
    /// <summary>
    /// The db set
    /// </summary>
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository"/> class
    /// </summary>
    /// <param name="context">The context</param>
    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing an enumerable of t</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <returns>A task containing the</returns>
    public virtual async Task<T> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException();
    }

    /// <summary>
    /// Adds the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <exception cref="KeyNotFoundException"></exception>
    public virtual async Task DeleteByIdAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        _dbSet.Remove(entity ?? throw new KeyNotFoundException());
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the entity
    /// </summary>
    /// <param name="entity">The entity</param>
    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }
}