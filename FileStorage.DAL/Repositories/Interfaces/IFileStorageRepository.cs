using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Http;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Repositories.Interfaces;

public interface IFileStorageRepository
{
    Task CreateFileAsync(string path, byte[] streamedFileContent);
    Task<MemoryStream> ReadFileAsync(string path);
    void DeleteFile(string path);
    Task<byte[]> ProcessFormFileAsync(IFormFile formFile, long sizeLimit);
    File CreateFileItemFormFile(IFormFile formFile, Folder primaryFolder, string userId);
}