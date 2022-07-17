using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Repositories.Interfaces;

public interface IFilesRepository : IRepository<int, File>
{
    
}