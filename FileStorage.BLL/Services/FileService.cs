using AutoMapper;
using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Validation;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;


namespace FileStorage.BLL.Services;

/// <summary>

/// The file service class

/// </summary>

/// <seealso cref="IFileService"/>

public class FileService : IFileService
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// The mapper profile
    /// </summary>
    private readonly IMapper _mapperProfile;
    /// <summary>
    /// The get current directory
    /// </summary>
    private readonly string _targetPath = Directory.GetParent(Directory.GetCurrentDirectory()) + "/FileStorage.DAL/Storage/";
    /// <summary>
    /// The file size limit
    /// </summary>
    private const long FileSizeLimit = 500000000;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileService"/> class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapperProfile">The mapper profile</param>
    public FileService(IUnitOfWork unitOfWork, IMapper mapperProfile)
    {
        _unitOfWork = unitOfWork;
        _mapperProfile = mapperProfile;
    }

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing an enumerable of file view model</returns>
    public async Task<IEnumerable<FileViewModel>> GetAllAsync()
    {
        var files = await _unitOfWork.FilesRepository.GetAllAsync();
        return _mapperProfile.Map<IEnumerable<FileViewModel>>(files);
    }

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the file view model</returns>
    public async Task<FileViewModel> GetByIdAsync(int id)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(id);
        return _mapperProfile.Map<FileViewModel>(file);
    }

    /// <summary>
    /// Uploads the user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="file">The file</param>
    /// <returns>The file dto</returns>
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
    
    /// <summary>
    /// Downloads the file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    /// <exception cref="FileStorageException">File for current user does not exist.</exception>
    /// <returns>A task containing the memory stream stream string content type string file name</returns>
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

    /// <summary>
    /// Updates the model
    /// </summary>
    /// <param name="model">The model</param>
    /// <exception cref="FileStorageException">No such item found</exception>
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

    /// <summary>
    /// Deletes the id
    /// </summary>
    /// <param name="id">The id</param>
    public async Task DeleteAsync(int id)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(id);
        await _unitOfWork.FilesRepository.DeleteByIdAsync(id);
        _unitOfWork.FileStorageRepository.DeleteFile(_targetPath + file.UserId + file.Path);
    }

    /// <summary>
    /// Gets the by filter using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing an enumerable of file view model</returns>
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

    /// <summary>
    /// Gets the by user using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns>The items</returns>
    public async Task<IEnumerable<FileViewModel>> GetByUserAsync(string userId)
    {
        var items = await GetAllAsync();
        items = items.Where(file => file.UserId == userId);
        return items;
    }
    
    /// <summary>
    /// Moves the file recycle bin using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    public async Task MoveFileRecycleBinAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsRecycled = true;
        editModel.IsPublic = false;

        await UpdateAsync(editModel);
    }

    /// <summary>
    /// Restores the recycled file using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    public async Task RestoreRecycledFileAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsRecycled = false;

        await UpdateAsync(editModel);
    }

    /// <summary>
    /// Changes the file visibility using the specified file id
    /// </summary>
    /// <param name="fileId">The file id</param>
    public async Task ChangeFileVisibilityAsync(int fileId)
    {
        var file = await _unitOfWork.FilesRepository.GetByIdAsync(fileId);
        var editModel = _mapperProfile.Map<FileEditModel>(file);
        editModel.IsPublic = !editModel.IsPublic;

        await UpdateAsync(editModel);
    }

    /// <summary>
    /// Gets the by file name using the specified file name
    /// </summary>
    /// <param name="fileName">The file name</param>
    /// <param name="userId">The user id</param>
    /// <exception cref="FileStorageException">No such file</exception>
    /// <returns>A task containing the file view model</returns>
    public async Task<FileViewModel> GetByFileName(string fileName, string userId)
    {
        var files = await GetByUserAsync(userId);
        var file = files.SingleOrDefault(item => item.Name == fileName);
        return file ?? throw new FileStorageException("No such file");
    }
}