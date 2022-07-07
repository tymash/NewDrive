using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Repositories;

public class FilesRepository : BaseRepository<int, File>, IFilesRepository
{
    public FilesRepository(DbContext context) : base(context)
    {
    }
}