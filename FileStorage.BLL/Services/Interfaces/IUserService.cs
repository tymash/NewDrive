using FileStorage.BLL.Models;

namespace FileStorage.BLL.Services.Interfaces;

public interface IUserService : ICrud<string, UserModel>
{
    Task DeleteAsync(string userId);
    Task RegisterAsync(UserModel model);
    Task LoginAsync(UserModel model); 
    Task LogoutAsync(); 
    Task EditUserDataAsync(UserModel model);
    Task ChangeUserPasswordAsync(UserModel model);
}