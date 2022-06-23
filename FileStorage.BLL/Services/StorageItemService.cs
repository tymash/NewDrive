using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Services.Interfaces;
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

    public async Task<IEnumerable<StorageItemModel>> GetAllAsync()
    {
        var storageItems = await _unitOfWork.StorageItemsRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<StorageItemModel>>(storageItems);
    }

    public async Task<StorageItemModel> GetByIdAsync(int id)
    {
        var storageItem = await _unitOfWork.StorageItemsRepository.GetByIdAsync(id);
        return _mapperProfile.Map<StorageItemModel>(storageItem);
    }

    public async Task AddAsync(StorageItemModel model)
    {
        var storageItem = _mapperProfile.Map<StorageItem>(model);
        await _unitOfWork.StorageItemsRepository.AddAsync(storageItem);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(StorageItemModel model)
    {
        var storageItem = _mapperProfile.Map<StorageItem>(model);
        _unitOfWork.StorageItemsRepository.Update(storageItem);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.StorageItemsRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<StorageItemModel>> GetByUserIdAsync(string userId)
    {
        var storageItems = await _unitOfWork.StorageItemsRepository.GetAllAsync();
        storageItems = storageItems.Where(storageItem => storageItem.UserId == userId);
        return _mapperProfile.Map<IEnumerable<StorageItemModel>>(storageItems);
    }

    public async Task<IEnumerable<StorageItemModel>> GetByFilterAsync(FilterModel model)
    {
        var storageItems = await _unitOfWork.StorageItemsRepository.GetAllAsync();
        if (model.Name != null)
        {
            storageItems = storageItems.Where(folder => folder.Name.Contains(model.Name));
        } 
        if (model.MinDate != null)
        {
            storageItems = storageItems.Where(folder => folder.CreatedOn >= model.MinDate);
        }
        if (model.MaxDate != null)
        {
            storageItems = storageItems.Where(folder => folder.CreatedOn <= model.MaxDate);
        }
        
        return _mapperProfile.Map<IEnumerable<StorageItemModel>>(storageItems);
    }
}