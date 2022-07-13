using System.Data;
using System.Net;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using StorageFile = FileStorage.DAL.Entities.File;
using File = System.IO.File;

namespace FileStorage.DAL.Repositories;

public class StorageRepository : IFileStorageRepository
{
    public async Task CreateFileAsync(string path, byte[] streamedFileContent)
    {
        if (path == null)
            throw new UnauthorizedAccessException("Path must not be null");

        if (File.Exists(path))
            throw new UnauthorizedAccessException("File has been already exists.");
        
        if (!Uri.TryCreate(path, UriKind.Absolute, out var pathUri))
        {
            throw new UnauthorizedAccessException(pathUri + "Wrong path");
        }

        using (var targetStream = File.Create(path))
        {
            await targetStream.WriteAsync(streamedFileContent);
        }
    }

    public async Task<MemoryStream> ReadFileAsync(string path)
    {
        if (path == null)
            throw new UnauthorizedAccessException("Path must not be null");

        if (!File.Exists(path))
            throw new FileNotFoundException("Current file does not exists");

        var memoryStream = new MemoryStream();

        using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memoryStream);
        }

        return memoryStream;
    }

    public void DeleteFile(string path)
    {
        if (path == null)
            throw new ArgumentException("Path must not be null");

        if (!File.Exists(path))
            throw new ArgumentException("Current file does not exists.");

        File.Delete(path);
    }

    public async Task<byte[]> ProcessFormFileAsync(IFormFile formFile, long sizeLimit)
    {
        var name = WebUtility.HtmlEncode(formFile.FileName);

        if (formFile.Length == 0)
            throw new FileNotFoundException("FormFile is empty.");

        if (formFile.Length > sizeLimit)
            throw new InsufficientMemoryException($"File: {name} exceeds {sizeLimit / 1048576:N1} MB.");

        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);

            if (memoryStream.Length == 0)
                throw new EndOfStreamException($"File: {name} is empty from MemoryStream.");

            return memoryStream.ToArray();
        }
    }

    public StorageFile CreateFileItemFormFile(IFormFile formFile, Folder primaryFolder, string userId)
    {
        var fileItem = new StorageFile
        {
            Name = WebUtility.HtmlEncode(formFile.FileName),
            Extension = Path.GetExtension(formFile.FileName),
            CreatedOn = DateTime.Now,
            IsRecycled = false,
            IsPublic = false,
            Size = formFile.Length,
            UserId = userId,
            ParentFolder = primaryFolder
        };
        fileItem.Path = Path.Combine(fileItem.ParentFolder.Path, fileItem.Name);

        return fileItem;
    }
    
    
}