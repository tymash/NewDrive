using FileStorage.DAL.Context;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

/// <summary>

/// The unit of work class

/// </summary>

/// <seealso cref="IUnitOfWork"/>

public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// The users repository
    /// </summary>
    private IUsersRepository? _usersRepository;
    /// <summary>
    /// The files repository
    /// </summary>
    private IFilesRepository? _filesRepository;
    /// <summary>
    /// The file storage repository
    /// </summary>
    private IFileStorageRepository? _fileStorageRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class
    /// </summary>
    public UnitOfWork()
    {
        _context = new AppDbContext();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class
    /// </summary>
    /// <param name="context">The context</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the value of the users repository
    /// </summary>
    public IUsersRepository UsersRepository
    {
        get
        {
            _usersRepository ??= new UsersRepository(_context);
            return _usersRepository;
        }
    }

    /// <summary>
    /// Gets the value of the files repository
    /// </summary>
    public IFilesRepository FilesRepository
    {
        get
        {
            _filesRepository ??= new FilesRepository(_context);
            return _filesRepository;
        }
    }

    /// <summary>
    /// Gets the value of the file storage repository
    /// </summary>
    public IFileStorageRepository FileStorageRepository
    {
        get
        {
            _fileStorageRepository ??= new StorageRepository();
            return _fileStorageRepository;
        }
    }

    /// <summary>
    /// Saves this instance
    /// </summary>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}