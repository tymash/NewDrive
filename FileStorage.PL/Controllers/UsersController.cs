using System.Net;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

/// <summary>

/// The users controller class

/// </summary>

/// <seealso cref="ControllerBase"/>

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    /// <summary>
    /// The user service
    /// </summary>
    private readonly IUserService _userService;
    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<User> _userManager;
    
    /// <summary>
    /// The register validator
    /// </summary>
    private readonly IValidator<UserRegisterModel> _registerValidator;
    /// <summary>
    /// The login validator
    /// </summary>
    private readonly IValidator<UserLoginModel> _loginValidator;
    /// <summary>
    /// The edit validator
    /// </summary>
    private readonly IValidator<UserEditModel> _editValidator;
    /// <summary>
    /// The change password validator
    /// </summary>
    private readonly IValidator<UserChangePasswordModel> _changePasswordValidator;


    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class
    /// </summary>
    /// <param name="userService">The user service</param>
    /// <param name="userManager">The user manager</param>
    /// <param name="registerValidator">The register validator</param>
    /// <param name="loginValidator">The login validator</param>
    /// <param name="editValidator">The edit validator</param>
    /// <param name="changePasswordValidator">The change password validator</param>
    public UsersController(IUserService userService, UserManager<User> userManager, IValidator<UserRegisterModel> registerValidator, IValidator<UserLoginModel> loginValidator,
        IValidator<UserEditModel> editValidator, IValidator<UserChangePasswordModel> changePasswordValidator
    )
    {
        _userService = userService;
        _userManager = userManager;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _editValidator = editValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    // GET: api/users
    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing the action result</returns>
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return new ObjectResult(users);
    }

    // GET: api/users/1
    /// <summary>
    /// Gets the by id using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns>A task containing the action result</returns>
    [HttpGet("{userId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> GetById(string userId)
    {
        var userModel = await _userService.GetByIdAsync(userId);

        return new ObjectResult(userModel);
    }
    
    // GET: api/users/current
    /// <summary>
    /// Gets the current user
    /// </summary>
    /// <returns>A task containing the action result</returns>
    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = _userManager.GetUserId(User);
        var userModel = await _userService.GetByIdAsync(userId);

        return new ObjectResult(userModel);
    }

    // POST: api/users/register
    /// <summary>
    /// Registers the user model
    /// </summary>
    /// <param name="userModel">The user model</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] UserRegisterModel userModel)
    {
        await _registerValidator.ValidateAndThrowAsync(userModel);
        
        var result = await _userService.RegisterAsync(userModel);

        return new ObjectResult(result);
    }

    // POST: api/users/login
    /// <summary>
    /// Logins the user model
    /// </summary>
    /// <param name="userModel">The user model</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginModel userModel)
    {
        await _loginValidator.ValidateAndThrowAsync(userModel);
        
        var result = await _userService.LoginAsync(userModel);

        return new ObjectResult(result);
    }

    // POST: api/users/logout
    /// <summary>
    /// Logouts this instance
    /// </summary>
    /// <returns>A task containing the action result</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _userService.LogoutAsync();

        return Ok();
    }

    // PUT: api/users/edit/1
    /// <summary>
    /// Changes the user data using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="userModel">The user model</param>
    /// <returns>A task containing the action result</returns>
    [HttpPut("edit/{userId}")]
    [Authorize]
    public async Task<ActionResult> ChangeUserData(string userId, [FromBody] UserEditModel userModel)
    {
        userModel.Id = userId;
        var currentUser = await _userManager.GetUserAsync(User);

        await _editValidator.ValidateAndThrowAsync(userModel);

        if (currentUser.Id != userId && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int) HttpStatusCode.Forbidden);

        await _userService.EditUserDataAsync(userModel);

        return Ok();
    }

    // PUT: api/users/change-password/
    /// <summary>
    /// Changes the user password using the specified user model
    /// </summary>
    /// <param name="userModel">The user model</param>
    /// <returns>A task containing the action result</returns>
    [HttpPut("change-password/")]
    [Authorize]
    public async Task<ActionResult> ChangeUserPassword([FromBody] UserChangePasswordModel userModel)
    {
        await _changePasswordValidator.ValidateAndThrowAsync(userModel);
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
        
        if (currentUser.Id != userModel.Id && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int) HttpStatusCode.Forbidden);

        await _userService.ChangeUserPasswordAsync(userModel);

        return Ok();
    }
}