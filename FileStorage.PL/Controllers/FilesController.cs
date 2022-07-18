using System.Net;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
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
    [Authorize]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetByFilter([FromQuery] FilterModel filterModel)
    {
        var userId = _userManager.GetUserId(User);
        filterModel.UserId = userId;
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

    // POST: api/items/upload
    [HttpPost("upload")]
    [RequestSizeLimit(500_000_000)]
    [Authorize]
    public async Task<IActionResult> Upload()
    {
        var userId = _userManager.GetUserId(User);
        var files = Request.Form.Files;
        var filesModels = new List<FileViewModel>();
        

        foreach (var file in files)
        {
            filesModels.Add(await _fileService.UploadAsync(userId, file));
        }

        return Ok(filesModels);
    }
    
    [HttpGet("download/{fileId}"), DisableRequestSizeLimit]
    [Authorize]
    public async Task<IActionResult> Download(int fileId)
    {
        try
        {
            var result = await _fileService.DownloadAsync(fileId);

            return File(result.stream, result.contentType, result.fileName);
        }
        catch (FileStorageException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("download/shared/{fileId}"), DisableRequestSizeLimit]
    public async Task<IActionResult> PublicDownload(int fileId)
    {
        var file = await _fileService.GetByIdAsync(fileId);
        if (!file.IsPublic) return Unauthorized();
        try
        {
            var result = await _fileService.DownloadAsync(fileId);

            return File(result.stream, result.contentType, result.fileName);
        }
        catch (FileStorageException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    // PUT: api/items/update/1
    [HttpPut("update/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Update(int itemId, [FromBody] FileEditModel fileModel)
    {
        // fileModel.Id = itemId;
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.Files.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _fileService.UpdateAsync(fileModel);

        return Ok();
    }
    
    // POST: api/items/delete/1
    [HttpDelete("delete/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int itemId)
    {
        var userId = _userManager.GetUserId(User);
        var file = await _fileService.GetByIdAsync(itemId);

        if (userId != file.UserId) return Unauthorized();

        await _fileService.DeleteAsync(itemId);

        return Ok();
    }
    
    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetByUser()
    {
        var userId = _userManager.GetUserId(User);

        var files = await _fileService.GetByUserAsync(userId);

        return new ObjectResult(files);
    }
    
    [Authorize]
    [HttpPut("{fileId}/recycle")]
    public async Task<IActionResult> MoveToRecycleBinAsync(int fileId)
    {
        var userId = _userManager.GetUserId(User);
        var file = await _fileService.GetByIdAsync(fileId);
        if (userId != file.UserId) return Unauthorized();

        try
        {
            await _fileService.MoveFileRecycleBinAsync(fileId);

            return Ok();
        }
        catch
        {
            return NotFound();
        }
    }
    
    [Authorize]
    [HttpPut("{fileId}/restore")]
    public async Task<IActionResult> RestoreFileAsync(int fileId)
    {
        var userId = _userManager.GetUserId(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (userId != file.Result.UserId) return Unauthorized();

        try
        {
            await _fileService.RestoreRecycledFileAsync(fileId);

            return Ok();
        }
        catch
        {
            return NotFound();
        }
    }
    
    [Authorize]
    [HttpPut("{fileId}/changePublic")]
    public async Task<IActionResult> ChangeVisibilityAsync(int fileId)
    {
        var userId = _userManager.GetUserId(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (userId != file.Result.UserId) return Unauthorized();
       

        try
        {
            await _fileService.ChangeFileVisibilityAsync(fileId);

            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

}