using FileStorage.DAL.Context;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUsersRepository? _usersRepository;
    private IStorageItemsRepository? _storageItemsRepository;
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

    public IStorageItemsRepository StorageItemsRepository
    {
        get
        {
            _storageItemsRepository ??= new StorageItemsRepository(_context);
            return _storageItemsRepository;
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