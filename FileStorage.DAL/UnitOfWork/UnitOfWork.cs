using FileStorage.DAL.Context;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUsersRepository? _usersRepository;
    private IFilesRepository? _filesRepository;
    private IFoldersRepository? _foldersRepository;

    public UnitOfWork()
    {
        _context = new AppDbContext();
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUsersRepository UsersRepository
    {
        get
        {
            _usersRepository ??= new UsersRepository(_context);
            return _usersRepository;
        }
    }

    public IFilesRepository FilesRepository
    {
        get
        {
            _filesRepository ??= new FilesRepository(_context);
            return _filesRepository;
        }
    }

    public IFoldersRepository FoldersRepository
    {
        get
        {
            _foldersRepository ??= new FoldersRepository(_context);
            return _foldersRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}