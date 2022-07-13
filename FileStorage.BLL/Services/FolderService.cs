using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.FolderModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;

namespace FileStorage.BLL.Services;

public class FolderService : IFolderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapperProfile;

    public FolderService(IUnitOfWork unitOfWork, IMapper mapperProfile)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
    }
    
    public async Task<IEnumerable<FolderViewModel>> GetAllAsync()
    {
        var folders = await _unitOfWork.FoldersRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<FolderViewModel>>(folders);
    }

    public async Task<FolderViewModel> GetByIdAsync(int id)
    {
        var folder = await _unitOfWork.FoldersRepository.GetByIdAsync(id);
        return _mapperProfile.Map<FolderViewModel>(folder);
    }

    public async Task<FolderViewModel> AddAsync(FolderCreateModel model)
    {
        if (model == null)
            throw new FileStorageException("No such folder found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.Path))
            throw new FileStorageException("Path is empty");

        var folder = _mapperProfile.Map<Folder>(model);
        await _unitOfWork.FoldersRepository.AddAsync(folder);
        await _unitOfWork.SaveAsync();

        return _mapperProfile.Map<FolderViewModel>(folder);
    }
    
    public async Task CreatePrimaryFolderAsync(string userId)
    {
        if (userId == null)
            throw new FileStorageException("No such user found");

        var folder = new FolderCreateModel
        {
            IsPrimaryFolder = true,
            Name = "PrimaryFolder",
            Path = "/",
            UserId = userId
        };

        await AddAsync(folder);
    }

    public async Task UpdateAsync(FolderEditModel model)
    {
        if (model == null)
            throw new FileStorageException("No such folder found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.Path))
            throw new FileStorageException("Path is empty");
        
        var folder = _mapperProfile.Map<Folder>(model);
        _unitOfWork.FoldersRepository.Update(folder);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.FoldersRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<FolderViewModel>> GetByFilterAsync(FilterModel model)
    {
        var folders = await _unitOfWork.FoldersRepository.GetAllAsync();
        if (model.Name != null)
        {
            folders = folders.Where(folder => folder.Name.Contains(model.Name));
        } 
        folders = model.DateSort switch
        {
            Sort.Ascending => folders.OrderBy(f => f.CreatedOn),
            Sort.Descending => folders.OrderByDescending(f => f.CreatedOn),
            _ => folders
        };
        
        folders = model.NameSort switch
        {
            Sort.Ascending => folders.OrderBy(f => f.Name),
            Sort.Descending => folders.OrderByDescending(f => f.Name),
            _ => folders
        };
        
        return _mapperProfile.Map<IEnumerable<FolderViewModel>>(folders);
    }

    public async Task<IEnumerable<FileViewModel>> GetItemsInFolder(int id)
    {
        var folder = await _unitOfWork.FoldersRepository.GetByIdAsync(id);
        var items = folder.Files;
        return _mapperProfile.Map<IEnumerable<FileViewModel>>(items);
    }
}