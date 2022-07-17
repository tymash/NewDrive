using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.DAL.UnitOfWork;

public interface IUnitOfWork
{
    public IUsersRepository UsersRepository { get; }
    public IFilesRepository FilesRepository { get; }
    public IFileStorageRepository FileStorageRepository { get; }
    public Task SaveAsync();
}