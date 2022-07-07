using System.Net;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

[Route("api/items")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly UserManager<User> _userManager;

    public FilesController(IFileService fileService, UserManager<User> userManager)
    {
        _fileService = fileService;
        _userManager = userManager;
    }
    
    // GET: api/items
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetByFilter([FromQuery] FilterModel filterModel)
    {
        var files = await _fileService.GetByFilterAsync(filterModel);

        return new ObjectResult(files);
    }

    // GET: api/items/1
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetById(int id)
    {
        var file = await _fileService.GetByIdAsync(id);

        return new ObjectResult(file);
    }

    // POST: api/items/add
    [HttpPost("add")]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] FileCreateModel fileModel)
    {
        if (User.Identity == null) return Unauthorized();
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        fileModel.UserId = currentUser.Id;

        var result = await _fileService.AddAsync(fileModel);

        return new ObjectResult(result);
    }
    
    // PUT: api/items/update/1
    [HttpPut("update/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Update(int itemId, [FromBody] FileEditModel fileModel)
    {
        fileModel.Id = itemId;
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.Files.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _fileService.UpdateAsync(fileModel);

        return Ok();
    }
    
    // POST: api/items/delete/1
    [HttpPut("delete/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int itemId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.Files.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _fileService.DeleteAsync(itemId);

        return Ok();
    }
    
}