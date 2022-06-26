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
    public static void SeedUsers(IConfiguration configuration, UserManager<User> userManager)
    {
        const string mainAdminUsername = "tymash";

        if (userManager.FindByNameAsync(mainAdminUsername).Result != null) return;
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Tymofii",
            Surname = "Karakash",
            UserName = "tymash",
            Email = "karakash.tymofiy@gmail.com"
        };

        userManager.CreateAsync(user, "ash2001").Wait();
        userManager.AddToRoleAsync(user, "Administrator").Wait();
    }

}