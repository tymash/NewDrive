using FileStorage.BLL.Models;

namespace FileStorage.BLL.Services.Interfaces;

public interface IFolderService : ICrud<int, FolderModel>
{
    Task<IEnumerable<FolderModel>> GetByUserIdAsync(string userId);
    Task<IEnumerable<FolderModel>> GetByFilterAsync(FilterModel model);
}