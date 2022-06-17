using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL.Repositories;

public class StorageItemsRepository : BaseRepository<int, StorageItem>, IStorageItemsRepository
{
    public StorageItemsRepository(DbContext context) : base(context)
    {
    }
}