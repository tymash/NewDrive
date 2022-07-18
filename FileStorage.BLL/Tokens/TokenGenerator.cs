using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FileStorage.BLL.Tokens;

/// <summary>

/// The token generator class

/// </summary>

/// <seealso cref="ITokenGenerator"/>

public class TokenGenerator : ITokenGenerator
{
    /// <summary>
    /// The user manager
    /// </summary>
    private readonly UserManager<User> _userManager;
    
    /// <summary>
    /// The issuer
    /// </summary>
    private readonly string _issuer;
    /// <summary>
    /// The audience
    /// </summary>
    private readonly string _audience;
    /// <summary>
    /// The key
    /// </summary>
    private readonly string _key;
    /// <summary>
    /// The hours life time
    /// </summary>
    private const int HoursLifeTime = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenGenerator"/> class
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="userManager">The user manager</param>
    public TokenGenerator(IConfiguration configuration, UserManager<User> userManager)
    {
        _key = configuration["Jwt:Key"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _userManager = userManager;
    }

    /// <summary>
    /// Builds the new token using the specified user
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>A task containing the string</returns>
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
