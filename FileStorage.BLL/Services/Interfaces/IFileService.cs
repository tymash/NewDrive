using FileStorage.BLL.Models;
using FileStorage.BLL.Models.FileModels;
using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Services.Interfaces;

public interface IFileService
{
    Task<IEnumerable<FileViewModel>> GetAllAsync();
    Task<FileViewModel> GetByIdAsync(int id);
    Task<FileViewModel> AddAsync(FileCreateModel model);
    Task UpdateAsync(FileEditModel model);
    Task DeleteAsync(int id);
    Task<IEnumerable<FileViewModel>> GetByFilterAsync(FilterModel model);
}