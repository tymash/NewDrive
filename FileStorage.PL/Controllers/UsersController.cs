using System.Net;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    
    private readonly IValidator<UserRegisterModel> _registerValidator;
    private readonly IValidator<UserLoginModel> _loginValidator;
    private readonly IValidator<UserEditModel> _editValidator;
    private readonly IValidator<UserChangePasswordModel> _changePasswordValidator;


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
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return new ObjectResult(users);
    }

    // GET: api/users/1
    [HttpGet("{userId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> GetById(string userId)
    {
        var userModel = await _userService.GetByIdAsync(userId);

        return new ObjectResult(userModel);
    }
    
    // GET: api/users/current
    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = _userManager.GetUserId(User);
        var userModel = await _userService.GetByIdAsync(userId);

        return new ObjectResult(userModel);
    }

    // POST: api/users/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] UserRegisterModel userModel)
    {
        await _registerValidator.ValidateAndThrowAsync(userModel);
        
        var result = await _userService.RegisterAsync(userModel);

        return new ObjectResult(result);
    }

    // POST: api/users/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginModel userModel)
    {
        await _loginValidator.ValidateAndThrowAsync(userModel);
        
        var result = await _userService.LoginAsync(userModel);

        return new ObjectResult(result);
    }

    // POST: api/users/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _userService.LogoutAsync();

        return Ok();
    }

    // PUT: api/users/edit/1
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