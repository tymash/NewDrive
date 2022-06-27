using System.Net;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.PL.Controllers;

[Route("api/items")]
[ApiController]
public class StorageItemsController : ControllerBase
{
    private readonly IStorageItemService _storageItemService;
    private readonly UserManager<User> _userManager;

    public StorageItemsController(IStorageItemService storageItemService, UserManager<User> userManager)
    {
        _storageItemService = storageItemService;
        _userManager = userManager;
    }
    
    // GET: api/items
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<StorageItemViewModel>>> GetByFilter([FromQuery] FilterModel filterModel)
    {
        var storageItems = await _storageItemService.GetByFilterAsync(filterModel);

        return new ObjectResult(storageItems);
    }

    // GET: api/items/1
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<StorageItemViewModel>>> GetById(int id)
    {
        var storageItem = await _storageItemService.GetByIdAsync(id);

        return new ObjectResult(storageItem);
    }

    // POST: api/items/add
    [HttpPost("add")]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] StorageItemCreateModel storageItemModel)
    {
        if (User.Identity == null) return Unauthorized();
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        storageItemModel.UserId = currentUser.Id;

        var result = await _storageItemService.AddAsync(storageItemModel);

        return new ObjectResult(result);
    }
    
    // PUT: api/items/update/1
    [HttpPut("update/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Update(int itemId, [FromBody] StorageItemEditModel storageItemModel)
    {
        storageItemModel.Id = itemId;
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.StorageItems.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _storageItemService.UpdateAsync(storageItemModel);

        return Ok();
    }
    
    // POST: api/items/delete/1
    [HttpPut("delete/{itemId}")]
    [Authorize]
    public async Task<ActionResult> Delete(int itemId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

        if (currentUser.StorageItems.All(si => si.Id != itemId) && !await _userManager.IsInRoleAsync(currentUser, "Administrator"))
            return StatusCode((int)HttpStatusCode.Forbidden);

        await _storageItemService.DeleteAsync(itemId);

        return Ok();
    }
    
}