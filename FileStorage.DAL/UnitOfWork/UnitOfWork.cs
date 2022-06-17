using FileStorage.DAL.Context;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public IUsersRepository UsersRepository { get; }
    public IStorageItemsRepository StorageItemsRepository { get; }
    public IFoldersRepository FoldersRepository { get; }
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, IUsersRepository usersRepository,
        IStorageItemsRepository storageItemsRepository, IFoldersRepository foldersRepository)
    {
        _context = context;
        StorageItemsRepository = storageItemsRepository;
        UsersRepository = usersRepository;
        FoldersRepository = foldersRepository;
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}