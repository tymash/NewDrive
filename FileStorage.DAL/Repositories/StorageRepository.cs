using System.Data;
using System.Net;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using StorageFile = FileStorage.DAL.Entities.File;
using File = System.IO.File;

namespace FileStorage.DAL.Repositories;

/// <summary>

/// The storage repository class

/// </summary>

/// <seealso cref="IFileStorageRepository"/>

public class StorageRepository : IFileStorageRepository
{
    /// <summary>
    /// Creates the file using the specified path
    /// </summary>
    /// <param name="path">The path</param>
    /// <param name="streamedFileContent">The streamed file content</param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="UnauthorizedAccessException">File has been already exists.</exception>
    /// <exception cref="UnauthorizedAccessException">Path must not be null</exception>
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

    /// <summary>
    /// Reads the file using the specified path
    /// </summary>
    /// <param name="path">The path</param>
    /// <exception cref="FileNotFoundException">Current file does not exists</exception>
    /// <exception cref="UnauthorizedAccessException">Path must not be null</exception>
    /// <returns>The memory stream</returns>
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

    /// <summary>
    /// Deletes the file using the specified path
    /// </summary>
    /// <param name="path">The path</param>
    /// <exception cref="ArgumentException">Current file does not exists.</exception>
    /// <exception cref="ArgumentException">Path must not be null</exception>
    public void DeleteFile(string path)
    {
        if (path == null)
            throw new ArgumentException("Path must not be null");

        if (!File.Exists(path))
            throw new ArgumentException("Current file does not exists.");

        File.Delete(path);
    }

    /// <summary>
    /// Processes the form file using the specified form file
    /// </summary>
    /// <param name="formFile">The form file</param>
    /// <param name="sizeLimit">The size limit</param>
    /// <exception cref="InsufficientMemoryException">File: {name} exceeds {sizeLimit / 1048576:N1} MB.</exception>
    /// <exception cref="EndOfStreamException">File: {name} is empty from MemoryStream.</exception>
    /// <exception cref="FileNotFoundException">FormFile is empty.</exception>
    /// <returns>A task containing the byte array</returns>
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

    /// <summary>
    /// Creates the file item form file using the specified form file
    /// </summary>
    /// <param name="formFile">The form file</param>
    /// <param name="userId">The user id</param>
    /// <returns>The file item</returns>
    public StorageFile CreateFileItemFormFile(IFormFile formFile, string userId)
    {
        var fileItem = new StorageFile
        {
            Name = WebUtility.HtmlEncode(formFile.FileName),
            Extension = Path.GetExtension(formFile.FileName),
            CreatedOn = DateTime.Now,
            IsRecycled = false,
            IsPublic = false,
            Size = formFile.Length,
            UserId = userId
        };
        fileItem.Path = Path.Combine("/", fileItem.Name);

        return fileItem;
    }
    
    
}