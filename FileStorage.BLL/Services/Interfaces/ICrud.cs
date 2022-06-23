namespace FileStorage.BLL.Services.Interfaces;

public interface ICrud<TId, TModel> where TModel : class
{
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TModel> GetByIdAsync(TId id);
    Task AddAsync(TModel model);
    Task UpdateAsync(TModel model);
    Task DeleteAsync(TId id);
}