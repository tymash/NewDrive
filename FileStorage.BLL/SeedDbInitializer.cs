using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.BLL;

/// <summary>

/// The seed db initializer class

/// </summary>

public static class SeedDbInitializer
{
    /// <summary>
    /// Seeds the roles using the specified role manager
    /// </summary>
    /// <param name="roleManager">The role manager</param>
    public static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.RoleExistsAsync("User").Result)
            roleManager.CreateAsync(new IdentityRole("User")).Wait();

        if (!roleManager.RoleExistsAsync("Administrator").Result)
            roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
    }
    /// <summary>
    /// Seeds the users using the specified user manager
    /// </summary>
    /// <param name="userManager">The user manager</param>
    public static void SeedUsers(UserManager<User> userManager)
    {
        const string mainAdminEmail = "tymash@email.com";

        if (userManager.FindByEmailAsync(mainAdminEmail).Result != null) return;
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Tymofiy",
            Surname = "Karakash",
            UserName = mainAdminEmail,
            Email = mainAdminEmail
        };

        userManager.CreateAsync(user, "ash2001").Wait();
        userManager.AddToRoleAsync(user, "Administrator").Wait();
    }

}