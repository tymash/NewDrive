using AutoMapper;
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
            throw new FileStorageException("Registration was unsuccesful");
        }
        
        await _userManager.AddToRoleAsync(user, "User");

        var userViewModel = _mapperProfile.Map<UserViewModel>(user);
        userViewModel.Token = await _tokenGenerator.BuildNewTokenAsync(user);

        return userViewModel;
    }

    public async Task<UserViewModel> LoginAsync(UserLoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            throw new FileStorageException("Incorrect email entered");
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new FileStorageException("Incorrect password entered");
        
        var userViewModel = _mapperProfile.Map<UserViewModel>(user);
        userViewModel.Token = await _tokenGenerator.BuildNewTokenAsync(user);

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
        
        user.Email = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
        user.UserName = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
        user.Name = string.IsNullOrEmpty(model.Name) ? user.Name : model.Name;
        user.Surname = string.IsNullOrEmpty(model.Surname) ? user.Surname : model.Surname;
        
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
}