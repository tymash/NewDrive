using System.Net;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
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

    public UsersController(IUserService userService, UserManager<User> userManager)
    {
        _userService = userService;
        _userManager = userManager;
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

    // POST: api/users/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] UserRegisterModel userModel)
    {
        var result = await _userService.RegisterAsync(userModel);

        return new ObjectResult(result);
    }

    // POST: api/users/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UserLoginModel userModel)
    {
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
    public async Task<ActionResult<UserEditModel>> ChangeUserData(string userId, [FromBody] UserEditModel userModel)
    {
        userModel.Id = userId;
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);


        if (currentUser.Id != userId && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int) HttpStatusCode.Forbidden);

        await _userService.EditUserDataAsync(userModel);

        return CreatedAtAction("GetById", new {userId = userId}, userModel);
    }

    // PUT: api/users/change-password/1
    [HttpPut("change-password/{userId}")]
    [Authorize]
    public async Task<ActionResult> ChangeUserPassword([FromBody] UserChangePasswordModel userModel)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
        
        if (currentUser.Id != userModel.Id && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int) HttpStatusCode.Forbidden);

        await _userService.ChangeUserPasswordAsync(userModel);

        return Ok();
    }
    
    // GET: api/users/1/userFolders
    [HttpGet("{userId}/userFolders")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FolderViewModel>>> GetUserFolders(string userId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
        
        if (currentUser.Id != userId && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        var folders = await _userService.GetUserFoldersAsync(userId);

        return new ObjectResult(folders);
    }
    
    // GET: api/users/1/userFolders
    [HttpGet("{userId}/userFiles")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetUserItems(string userId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
        
        if (currentUser.Id != userId && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        var folders = await _userService.GetUserItemsAsync(userId);

        return new ObjectResult(folders);
    }
}