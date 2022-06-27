using System.Net;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

[Route("api/folders")]
[ApiController]
public class FoldersController : ControllerBase
{
    private readonly IFolderService _folderService;
    private readonly UserManager<User> _userManager;

    public FoldersController(IFolderService folderService, UserManager<User> userManager)
    {
        _folderService = folderService;
        _userManager = userManager;
    }


    // GET: api/folders
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<FolderViewModel>>> GetByFilter([FromQuery] FilterModel filterModel)
    {
        var folders = await _folderService.GetByFilterAsync(filterModel);

        return new ObjectResult(folders);
    }

    // GET: api/folders/1
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FolderViewModel>>> GetById(int id)
    {
        var folder = await _folderService.GetByIdAsync(id);

        return new ObjectResult(folder);
    }

    // POST: api/folders/add
    [HttpPost("add")]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] FolderCreateModel folderModel)
    {
        if (User.Identity == null) return Unauthorized();
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        folderModel.UserId = currentUser.Id;

        var result = await _folderService.AddAsync(folderModel);

        return new ObjectResult(result);
    }
    
    // PUT: api/folders/update/1
    [HttpPut("update/{folderId}")]
    [Authorize]
    public async Task<ActionResult> Update(int folderId, [FromBody] FolderEditModel folderModel)
    {
        folderModel.Id = folderId;
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.Folders.All(f => f.Id != folderId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _folderService.UpdateAsync(folderModel);

        return Ok();
    }
    
    // POST: api/folders/delete/1
    [HttpPut("delete/{folderId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int folderId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.Folders.All(f => f.Id != folderId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _folderService.DeleteAsync(folderId);

        return Ok();
    }
}