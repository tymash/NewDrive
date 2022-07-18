using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace FileStorage.BLL.Services.Interfaces;

public interface IFileService
{
    Task<IEnumerable<FileViewModel>> GetAllAsync();
    Task<FileViewModel> GetByIdAsync(int id);
    Task<FileViewModel> UploadAsync(string userId, IFormFile file);
    Task UpdateAsync(FileEditModel model);
    Task DeleteAsync(int id);
    Task<IEnumerable<FileViewModel>> GetByFilterAsync(FilterModel model);
    Task<IEnumerable<FileViewModel>> GetByUserAsync(string userId);
    Task<(MemoryStream stream, string contentType, string fileName)> DownloadAsync(int fileId);
    Task ChangeFileVisibilityAsync(int fileId);
    Task MoveFileRecycleBinAsync(int fileId);
    Task RestoreRecycledFileAsync(int fileId);
    Task<FileViewModel> GetByFileName(string fileName, string userId);
}