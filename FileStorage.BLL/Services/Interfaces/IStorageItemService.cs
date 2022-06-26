using FileStorage.BLL.Models;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Services.Interfaces;

public interface IStorageItemService
{
    Task<IEnumerable<StorageItemViewModel>> GetAllAsync();
    Task<StorageItemViewModel> GetByIdAsync(int id);
    Task<StorageItemViewModel> AddAsync(StorageItemCreateModel model);
    Task UpdateAsync(StorageItemEditModel model);
    Task DeleteAsync(int id);
    Task<IEnumerable<StorageItemViewModel>> GetByFilterAsync(FilterModel model);
}