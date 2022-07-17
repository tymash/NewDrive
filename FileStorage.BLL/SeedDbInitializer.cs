using FileStorage.BLL.Services.Interfaces;
using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.BLL;

public static class SeedDbInitializer
{
    public static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.RoleExistsAsync("User").Result)
            roleManager.CreateAsync(new IdentityRole("User")).Wait();

        if (!roleManager.RoleExistsAsync("Administrator").Result)
            roleManager.CreateAsync(new IdentityRole("Administrator")).Wait();
    }
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