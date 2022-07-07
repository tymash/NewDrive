using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.BLL.Services;

public class FileService : IFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapperProfile;

    public FileService(IUnitOfWork unitOfWork, IMapper mapperProfile)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
    }

    public async Task<IEnumerable<FileViewModel>> GetAllAsync()
    {
        var files = await _unitOfWork.FilesRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<FileViewModel>>(files);
    }

    public async Task<FileViewModel> GetByIdAsync(int id)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(id);
        return _mapperProfile.Map<FileViewModel>(file);
    }

    public async Task<FileViewModel> AddAsync(FileCreateModel model)
    {
        if (model == null)
            throw new FileStorageException("No such item found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.Path))
            throw new FileStorageException("Path is empty");
        
        var file = _mapperProfile.Map<File>(model);
        await _unitOfWork.FilesRepository.AddAsync(file);
        await _unitOfWork.SaveAsync();

        return _mapperProfile.Map<FileViewModel>(file);
    }

    public async Task UpdateAsync(FileEditModel model)
    {
        if (model == null)
            throw new FileStorageException("No such item found");
        
        if (string.IsNullOrEmpty(model.Name))
            throw new FileStorageException("Name is empty");

        if (string.IsNullOrEmpty(model.Path))
            throw new FileStorageException("Path is empty");
        
        var file = _mapperProfile.Map<File>(model);
        _unitOfWork.FilesRepository.Update(file);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _unitOfWork.FilesRepository.DeleteByIdAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<FileViewModel>> GetByFilterAsync(FilterModel model)
    {
        var files = await _unitOfWork.FilesRepository.GetAllAsync();
        if (model.Name != null)
        {
            files = files.Where(folder => folder.Name.Contains(model.Name));
        }

        files = model.DateSort switch
        {
            Sort.Ascending => files.OrderBy(si => si.CreatedOn),
            Sort.Descending => files.OrderByDescending(si => si.CreatedOn),
            _ => files
        };
        
        files = model.NameSort switch
        {
            Sort.Ascending => files.OrderBy(si => si.Name),
            Sort.Descending => files.OrderByDescending(si => si.Name),
            _ => files
        };
        
        files = model.SizeSort switch
        {
            Sort.Ascending => files.OrderBy(si => si.Size),
            Sort.Descending => files.OrderByDescending(si => si.Size),
            _ => files
        };

        return _mapperProfile.Map<IEnumerable<FileViewModel>>(files);
    }
}