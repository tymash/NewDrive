using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.StorageItemModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;

namespace FileStorage.BLL.Services;

public class StorageItemService : IStorageItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapperProfile;

    public StorageItemService(IUnitOfWork unitOfWork, IMapper mapperProfile)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
    }

    public async Task<IEnumerable<StorageItemViewModel>> GetAllAsync()
    {
        var storageItems = await _unitOfWork.StorageItemsRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<StorageItemViewModel>>(storageItems);
    }

    public async Task<StorageItemViewModel> GetByIdAsync(int id)
    {
        var storageItem = await _unitOfWork.StorageItemsRepository.GetByIdAsync(id);
        return _mapperProfile.Map<StorageItemViewModel>(storageItem);
    }

    public async Task<StorageItemViewModel> AddAsync(StorageItemCreateModel model)
    {
        if (model == null)
            throw new FileStorageException("No such item found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.RelativePath))
            throw new FileStorageException("Path is empty");
        
        var storageItem = _mapperProfile.Map<StorageItem>(model);
        await _unitOfWork.StorageItemsRepository.AddAsync(storageItem);
        await _unitOfWork.SaveAsync();

        return _mapperProfile.Map<StorageItemViewModel>(storageItem);
    }

    public async Task UpdateAsync(StorageItemEditModel model)
    {
        if (model == null)
            throw new FileStorageException("No such item found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.RelativePath))
            throw new FileStorageException("Path is empty");
        
        var storageItem = _mapperProfile.Map<StorageItem>(model);
        _unitOfWork.StorageItemsRepository.Update(storageItem);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.StorageItemsRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<StorageItemViewModel>> GetByFilterAsync(FilterModel model)
    {
        var storageItems = await _unitOfWork.StorageItemsRepository.GetAllAsync();
        if (model.Name != null)
        {
            storageItems = storageItems.Where(folder => folder.Name.Contains(model.Name));
        }

        storageItems = model.DateSort switch
        {
            Sort.Ascending => storageItems.OrderBy(si => si.CreatedOn),
            Sort.Descending => storageItems.OrderByDescending(si => si.CreatedOn),
            _ => storageItems
        };
        
        storageItems = model.NameSort switch
        {
            Sort.Ascending => storageItems.OrderBy(si => si.Name),
            Sort.Descending => storageItems.OrderByDescending(si => si.Name),
            _ => storageItems
        };
        
        storageItems = model.SizeSort switch
        {
            Sort.Ascending => storageItems.OrderBy(si => si.Size),
            Sort.Descending => storageItems.OrderByDescending(si => si.Size),
            _ => storageItems
        };

        return _mapperProfile.Map<IEnumerable<StorageItemViewModel>>(storageItems);
    }
}