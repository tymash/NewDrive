using AutoMapper;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Tokens;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.BLL.Services;

/// <summary>

/// The user service class

/// </summary>

/// <seealso cref="IUserService"/>

public class UserService : IUserService
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// The mapper profile
    /// </summary>
    private readonly IMapper _mapperProfile;
    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<User> _userManager;
    /// <summary>
    /// The sign in manager
    /// </summary>
    private readonly SignInManager<User> _signInManager;
    /// <summary>
    /// The token generator
    /// </summary>
    private readonly ITokenGenerator _tokenGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapperProfile">The mapper profile</param>
    /// <param name="userManager">The user manager</param>
    /// <param name="signInManager">The sign in manager</param>
    /// <param name="tokenGenerator">The token generator</param>
    public UserService(IUnitOfWork unitOfWork, IMapper mapperProfile, UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
    }
    
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing an enumerable of user view model</returns>
    public async Task<IEnumerable<UserViewModel>> GetAllAsync()
    {
        var users = await _unitOfWork.UsersRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<UserViewModel>>(users);
    }

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the user view model</returns>
    public async Task<UserViewModel> GetByIdAsync(string id)
    {
        var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);
        return _mapperProfile.Map<UserViewModel>(user);
    }

    /// <summary>
    /// Deletes the id
    /// </summary>
    /// <param name="id">The id</param>
    public async Task DeleteAsync(string id)
    {
        await _unitOfWork.UsersRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    /// <summary>
    /// Registers the model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="FileStorageException">Registration was unsuccesful</exception>
    /// <returns>The user view model</returns>
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

    /// <summary>
    /// Logins the model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="FileStorageException">Incorrect email entered</exception>
    /// <exception cref="FileStorageException">Incorrect password entered</exception>
    /// <returns>The user view model</returns>
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

    /// <summary>
    /// Logouts this instance
    /// </summary>
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    /// <summary>
    /// Edits the user data using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="FileStorageException">No such user found</exception>
    /// <exception cref="FileStorageException">Update unsuccessful</exception>
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

    /// <summary>
    /// Changes the user password using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="FileStorageException">No such user found</exception>
    /// <exception cref="FileStorageException">Password change unsuccessful</exception>
    /// <exception cref="FileStorageException">Password is empty</exception>
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