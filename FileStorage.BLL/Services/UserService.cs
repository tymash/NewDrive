using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Tokens;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapperProfile;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenGenerator _tokenGenerator;

    public UserService(IUnitOfWork unitOfWork, IMapper mapperProfile, UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }
    
    public async Task<IEnumerable<UserViewModel>> GetAllAsync()
    {
        var users = await _unitOfWork.UsersRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<UserViewModel>>(users);
    }

    public async Task<UserViewModel> GetByIdAsync(string id)
    {
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);
        return _mapperProfile.Map<UserViewModel>(user);
    }

    public async Task DeleteAsync(string id)
    {
        await _unitOfWork.UsersRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<UserViewModel> RegisterAsync(UserRegisterModel model)
    {
        var user = _mapperProfile.Map<User>(model);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var exceptionText = result.Errors.Aggregate("User Creation Failed - Identity Exception. Errors were: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
            throw new FileStorageException(exceptionText);
        }
        
        await _userManager.AddToRoleAsync(user, "User");
        await _signInManager.SignInAsync(user, false);
        
        var userViewModel = _mapperProfile.Map<UserViewModel>(user);
        userViewModel.Token = _tokenGenerator.BuildNewToken(user);

        return userViewModel;

    }

    public async Task<UserViewModel> LoginAsync(UserLoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
        
        if (!result.Succeeded)
            throw new FileStorageException(result.ToString());
        
        var user = await _userManager.FindByNameAsync(model.UserName);
        var userViewModel = _mapperProfile.Map<UserViewModel>(user);
        userViewModel.Token = _tokenGenerator.BuildNewToken(user);

        return userViewModel;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task EditUserDataAsync(UserEditModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
            throw new FileStorageException("No such user found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");
        
        if (string.IsNullOrEmpty(model.Surname))
            throw new FileStorageException("Surname is empty");
        
        if (string.IsNullOrEmpty(model.UserName))
            throw new FileStorageException("Username is empty");
        
        if (string.IsNullOrEmpty(model.Email)) 
            throw new FileStorageException("Email is empty");

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new FileStorageException("Update unsuccessful");

    }

    public async Task ChangeUserPasswordAsync(UserChangePasswordModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
            throw new FileStorageException("No such user found");
        
        if (string.IsNullOrEmpty(model.Password))
            throw new FileStorageException("Password is empty");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

        if (!result.Succeeded)
            throw new FileStorageException("Password change unsuccessful");

    }
    
    public async Task<IEnumerable<FolderViewModel>> GetUserFoldersAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var folders = user.Folders;
        return _mapperProfile.Map<IEnumerable<FolderViewModel>>(folders);
    }
    
    public async Task<IEnumerable<FileViewModel>> GetUserItemsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var items = user.Files;
        return _mapperProfile.Map<IEnumerable<FileViewModel>>(items);
    }
}