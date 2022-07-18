using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Repositories;

/// <summary>

/// The files repository class

/// </summary>

/// <seealso cref="BaseRepository{int, File}"/>

/// <seealso cref="IFilesRepository"/>

public class FilesRepository : BaseRepository<int, File>, IFilesRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilesRepository"/> class
    /// </summary>
    /// <param name="context">The context</param>
    public FilesRepository(DbContext context) : base(context)
    {
    }
}