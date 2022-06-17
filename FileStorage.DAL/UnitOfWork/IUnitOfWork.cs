using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

public interface IUnitOfWork
{
    public IUsersRepository UsersRepository { get; }
    public IStorageItemsRepository StorageItemsRepository { get; }
    public IFoldersRepository FoldersRepository { get; }
    public Task SaveAsync();
}