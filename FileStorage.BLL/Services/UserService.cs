using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Services.Interfaces;
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

    public UserService(IUnitOfWork unitOfWork, IMapper mapperProfile, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _unitOfWork.UsersRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<UserModel> GetByIdAsync(string userId)
    {
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
        return _mapperProfile.Map<UserModel>(user);
    }

    public async Task AddAsync(UserModel model)
    {
        var user = _mapperProfile.Map<User>(model);
        await _unitOfWork.UsersRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(UserModel model)
    {
        var user = _mapperProfile.Map<User>(model);
        _unitOfWork.UsersRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(string userId)
    {
        await _unitOfWork.UsersRepository.DeleteByIdAsync(userId);
        await _unitOfWork.SaveAsync();
    }

    public async Task RegisterAsync(UserModel model)
    {
        var user = _mapperProfile.Map<User>(model);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            throw new FileStorageException("Registration unsuccessful");
        
        await _userManager.AddToRoleAsync(user, "User");
        await _signInManager.SignInAsync(user, false);
    }

    public async Task LoginAsync(UserModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
        
        if (!result.Succeeded)
            throw new FileStorageException("Login unsuccessful");
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task EditUserDataAsync(UserModel model)
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

    public async Task ChangeUserPasswordAsync(UserModel model)
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