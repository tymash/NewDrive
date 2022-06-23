using FileStorage.BLL.Models;
using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Services.Interfaces;

public interface IStorageItemService : ICrud<int, StorageItemModel>
{
    Task<IEnumerable<StorageItemModel>> GetByUserIdAsync(string userId);
    Task<IEnumerable<StorageItemModel>> GetByFilterAsync(FilterModel model);
}