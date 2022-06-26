using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FileStorage.BLL.Models.UserModels;
using FileStorage.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FileStorage.BLL.Tokens;

public class TokenGenerator : ITokenGenerator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private const int hoursLifeTime = 2;

    public TokenGenerator(IConfiguration configuration)
    {
        _key = configuration["Jwt:Key"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
    }

    public string BuildNewToken(User user)
    {
        var claims = new List<Claim> {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName)};

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var signingsCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(hoursLifeTime),
            signingCredentials: signingsCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwtToken);
    }
}
