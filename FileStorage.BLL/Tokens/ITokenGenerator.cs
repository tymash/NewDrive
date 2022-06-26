using FileStorage.DAL.Entities;

namespace FileStorage.BLL.Tokens;

public interface ITokenGenerator
{
    string BuildNewToken(User user);
}