using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.FolderModels;

namespace FileStorage.BLL.Services.Interfaces;

public interface IFolderService
{
    Task<IEnumerable<FolderViewModel>> GetAllAsync();
    Task<FolderViewModel> GetByIdAsync(int id);
    Task<FolderViewModel> AddAsync(FolderCreateModel model);
    Task CreatePrimaryFolderAsync(string userId);
    Task UpdateAsync(FolderEditModel model);
    Task DeleteAsync(int id);
    Task<IEnumerable<FolderViewModel>> GetByFilterAsync(FilterModel model);
    Task<IEnumerable<FileViewModel>> GetItemsInFolder(int id);
}