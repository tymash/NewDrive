using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FileStorage.BLL.Tokens;

public class TokenGenerator : ITokenGenerator
{
    private readonly UserManager<User> _userManager;
    
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private const int HoursLifeTime = 2;

    public TokenGenerator(IConfiguration configuration, UserManager<User> userManager)
    {
        _key = configuration["Jwt:Key"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _userManager = userManager;
    }

    public async Task<string> BuildNewTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim> {
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Role, roles.Single())};

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var signingsCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(HoursLifeTime),
            signingCredentials: signingsCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwtToken);
    }
}
