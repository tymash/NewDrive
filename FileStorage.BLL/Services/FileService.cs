using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;


namespace FileStorage.BLL.Services;

public class FileService : IFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapperProfile;
    private readonly string _targetPath = Directory.GetParent(Directory.GetCurrentDirectory()) + "/FileStorage.DAL/Storage/";
    private const long FileSizeLimit = 500000000;

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

    public async Task<FileViewModel> UploadAsync(string userId, IFormFile file)
    {
        var formFileContent = await _unitOfWork.FileStorageRepository.ProcessFormFileAsync(file, FileSizeLimit);
        var fileItem = _unitOfWork.FileStorageRepository.CreateFileItemFormFile(file, userId);

        
        var fullPath = _targetPath + userId;
        
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        await _unitOfWork.FileStorageRepository.CreateFileAsync(fullPath + fileItem.Path, formFileContent);

        await _unitOfWork.FilesRepository.AddAsync(fileItem);
        await _unitOfWork.SaveAsync();

        var fileDto = _mapperProfile.Map<FileViewModel>(fileItem);
        return fileDto;
    }
    
    public async Task<(MemoryStream stream, string contentType, string fileName)> DownloadAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);

        if (file == null)
            throw new FileStorageException($"File for current user does not exist.");


        var stream = await _unitOfWork.FileStorageRepository.ReadFileAsync(_targetPath + file.UserId + file.Path);
        stream.Position = 0;

        new FileExtensionContentTypeProvider().TryGetContentType(file.Name, out var contentType);
        return (stream, contentType, file.Name)!;
    }

    public async Task UpdateAsync(FileEditModel model)
    {
        if (model == null)
            throw new FileStorageException("No such item found");

        var file = await _unitOfWork.FilesRepository.GetByIdAsync(model.Id);
        file.Extension = string.IsNullOrEmpty(model.Extension) ? file.Extension : model.Extension;
        file.Name = string.IsNullOrEmpty(model.Name) ? file.Name : model.Name;
        file.Path = string.IsNullOrEmpty(model.Path) ? file.Path : model.Path;
        file.IsPublic = model.IsPublic;
        file.IsRecycled = model.IsRecycled;
        
        _unitOfWork.FilesRepository.Update(file);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(id);
        await _unitOfWork.FilesRepository.DeleteByIdAsync(id);
        _unitOfWork.FileStorageRepository.DeleteFile(_targetPath + file.UserId + file.Path);
    }

    public async Task<IEnumerable<FileViewModel>> GetByFilterAsync(FilterModel model)
    {
        var files = await _unitOfWork.FilesRepository.GetAllAsync();
        if (model.Name != null)
        {
            files = files.Where(file => file.Name.ToLower().Contains(model.Name.ToLower()));
        }
        
        if (model.IsRecycled != null)
        {
            files = files.Where(file => file.IsRecycled == model.IsRecycled);
        }
        
        if (model.IsPublic != null)
        {
            files = files.Where(file => file.IsPublic == model.IsPublic);
        }
        
        if (model.UserId != null)
        {
            files = files.Where(file => file.UserId == model.UserId);
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

    public async Task<IEnumerable<FileViewModel>> GetByUserAsync(string userId)
    {
        var items = await GetAllAsync();
        items = items.Where(file => file.UserId == userId);
        return items;
    }
    
    public async Task MoveFileRecycleBinAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsRecycled = true;
        editModel.IsPublic = false;

        await UpdateAsync(editModel);
    }

    public async Task RestoreRecycledFileAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsRecycled = false;

        await UpdateAsync(editModel);
    }

    public async Task ChangeFileVisibilityAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsPublic = !editModel.IsPublic;

        await UpdateAsync(editModel);
    }

    public async Task<FileViewModel> GetByFileName(string fileName, string userId)
    {
        var files = await GetByUserAsync(userId);
        var file = files.SingleOrDefault(item => item.Name == fileName);
        return file ?? throw new FileStorageException("No such file");
    }
}