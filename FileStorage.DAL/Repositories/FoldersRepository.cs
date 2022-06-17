using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

public class FoldersRepository : BaseRepository<int, Folder>, IFoldersRepository
{
    public FoldersRepository(DbContext context) : base(context)
    {
    }
}