using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Services.Interfaces;
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


    public async Task<IEnumerable<FolderModel>> GetAllAsync()
    {
        var folders = await _unitOfWork.FoldersRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<FolderModel>>(folders);
    }

    public async Task<FolderModel> GetByIdAsync(int id)
    {
        var folder = await _unitOfWork.FoldersRepository.GetByIdAsync(id);
        return _mapperProfile.Map<FolderModel>(folder);
    }

    public async Task AddAsync(FolderModel model)
    {
        var folder = _mapperProfile.Map<Folder>(model);
        await _unitOfWork.FoldersRepository.AddAsync(folder);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(FolderModel model)
    {
        var folder = _mapperProfile.Map<Folder>(model);
        _unitOfWork.FoldersRepository.Update(folder);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.FoldersRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<FolderModel>> GetByUserIdAsync(string userId)
    {
        var folders = await _unitOfWork.FoldersRepository.GetAllAsync();
        folders = folders.Where(folder => folder.UserId == userId);
        return _mapperProfile.Map<IEnumerable<FolderModel>>(folders);
    }

    public async Task<IEnumerable<FolderModel>> GetByFilterAsync(FilterModel model)
    {
        var folders = await _unitOfWork.FoldersRepository.GetAllAsync();
        if (model.Name != null)
        {
            folders = folders.Where(folder => folder.Name.Contains(model.Name));
        } 
        if (model.MinDate != null)
        {
            folders = folders.Where(folder => folder.CreatedOn >= model.MinDate);
        }
        if (model.MaxDate != null)
        {
            folders = folders.Where(folder => folder.CreatedOn <= model.MaxDate);
        }
        
        return _mapperProfile.Map<IEnumerable<FolderModel>>(folders);
    }
}