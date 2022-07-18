using System.Net;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

/// <summary>

/// The files controller class

/// </summary>

/// <seealso cref="ControllerBase"/>

[Route("api/items")]
[ApiController]
public class FilesController : ControllerBase
{
    /// <summary>
    /// The file service
    /// </summary>
    private readonly IFileService _fileService;
    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<User> _userManager;
    
    /// <summary>
    /// The create validator
    /// </summary>
    private readonly IValidator<FileCreateModel> _createValidator;
    /// <summary>
    /// The edit validator
    /// </summary>
    private readonly IValidator<FileEditModel> _editValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilesController"/> class
    /// </summary>
    /// <param name="fileService">The file service</param>
    /// <param name="userManager">The user manager</param>
    /// <param name="createValidator">The create validator</param>
    /// <param name="editValidator">The edit validator</param>
    public FilesController(IFileService fileService, UserManager<User> userManager, 
        IValidator<FileCreateModel> createValidator, IValidator<FileEditModel> editValidator)
    {
        _fileService = fileService;
        _userManager = userManager;
        _createValidator = createValidator;
        _editValidator = editValidator;
    }
    
    // GET: api/items
    /// <summary>
    /// Gets the by filter using the specified filter model
    /// </summary>
    /// <param name="filterModel">The filter model</param>
    /// <returns>A task containing an action result of i enumerable file view model</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetByFilter([FromQuery] FilterModel filterModel)
    {
        var files = await _fileService.GetByFilterAsync(filterModel);

        return new ObjectResult(files);
    }
    
    // GET: api/items
    /// <summary>
    /// Gets the user files by filter using the specified filter model
    /// </summary>
    /// <param name="filterModel">The filter model</param>
    /// <returns>A task containing an action result of i enumerable file view model</returns>
    [HttpGet("user")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetUserFilesByFilter([FromQuery] FilterModel filterModel)
    {
        var userId = _userManager.GetUserId(User);
        filterModel.UserId = userId;
        var files = await _fileService.GetByFilterAsync(filterModel);

        return new ObjectResult(files);
    }

    // GET: api/items/1
    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing an action result of i enumerable file view model</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<FileViewModel>>> GetById(int id)
    {
        var file = await _fileService.GetByIdAsync(id);

        return new ObjectResult(file);
    }

    // POST: api/items/upload
    /// <summary>
    /// Uploads this instance
    /// </summary>
    /// <returns>A task containing the action result</returns>
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
    
    /// <summary>
    /// Downloads the file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
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
    
    /// <summary>
    /// Publics the download using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
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
    
    // PUT: api/items/edit/1
    /// <summary>
    /// Edits the item id
    /// </summary>
    /// <param name="itemId">The item id</param>
    /// <param name="fileModel">The file model</param>
    /// <returns>A task containing the action result</returns>
    [HttpPut("edit/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Edit(int itemId, [FromBody] FileEditModel fileModel)
    {
        fileModel.Id = itemId;
        var currentUser = await _userManager.GetUserAsync(User);

        await _editValidator.ValidateAndThrowAsync(fileModel);

        if (currentUser.Files.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _fileService.UpdateAsync(fileModel);

        return Ok();
    }
    
    // POST: api/items/delete/1
    /// <summary>
    /// Deletes the file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
    [HttpDelete("delete/{fileId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int fileId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (currentUser.Id != file.Result.UserId && !await _userManager.IsInRoleAsync(currentUser, "Administrator")) 
            return Unauthorized();

        await _fileService.DeleteAsync(fileId);

        return Ok();
    }

    /// <summary>
    /// Moves the to recycle bin using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
    [Authorize]
    [HttpPut("{fileId}/recycle")]
    public async Task<IActionResult> MoveToRecycleBinAsync(int fileId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (currentUser.Id != file.Result.UserId && !await _userManager.IsInRoleAsync(currentUser, "Administrator")) 
            return Unauthorized();

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
    
    /// <summary>
    /// Restores the file using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
    [Authorize]
    [HttpPut("{fileId}/restore")]
    public async Task<IActionResult> RestoreFileAsync(int fileId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (currentUser.Id != file.Result.UserId && !await _userManager.IsInRoleAsync(currentUser, "Administrator")) 
            return Unauthorized();

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
    
    /// <summary>
    /// Changes the visibility using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <returns>A task containing the action result</returns>
    [Authorize]
    [HttpPut("{fileId}/changePublic")]
    public async Task<IActionResult> ChangeVisibilityAsync(int fileId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var file = _fileService.GetByIdAsync(fileId);
        if (currentUser.Id != file.Result.UserId && !await _userManager.IsInRoleAsync(currentUser, "Administrator")) 
            return Unauthorized();
       

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