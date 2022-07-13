using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Tokens;

public interface ITokenGenerator
{
    Task<string> BuildNewTokenAsync(User user);
}