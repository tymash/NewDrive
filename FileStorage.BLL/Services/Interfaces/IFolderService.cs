using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.StorageItemModels;

namespace FileStorage.BLL.Services.Interfaces;

public interface IFolderService
{
    Task<IEnumerable<FolderViewModel>> GetAllAsync();
    Task<FolderViewModel> GetByIdAsync(int id);
    Task<FolderViewModel> AddAsync(FolderCreateModel model);
    Task UpdateAsync(FolderEditModel model);
    Task DeleteAsync(int id);
    Task<IEnumerable<FolderViewModel>> GetByFilterAsync(FilterModel model);
    Task<IEnumerable<StorageItemViewModel>> GetItemsInFolder(int id);
}