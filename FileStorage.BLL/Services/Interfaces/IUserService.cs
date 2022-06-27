using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.BLL.Models.UserModels;

namespace FileStorage.BLL.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserViewModel>> GetAllAsync();
    Task<UserViewModel> GetByIdAsync(string id);
    Task<UserViewModel> RegisterAsync(UserRegisterModel model);
    Task<UserViewModel> LoginAsync(UserLoginModel model); 
    Task LogoutAsync(); 
    Task EditUserDataAsync(UserEditModel model);
    Task ChangeUserPasswordAsync(UserChangePasswordModel model);
    Task<IEnumerable<FolderViewModel>> GetUserFoldersAsync(string userId);
    Task<IEnumerable<StorageItemViewModel>> GetUserItemsAsync(string userId);

}