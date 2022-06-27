using AutoMapper;
using FileStorage.BLL;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Tokens;
using FileStorage.DAL.Context;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.PL;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutomapperProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(Configuration.GetConnectionString("f-Storage")).UseLazyLoadingProxies());

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        services.AddAuthorization();
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IStorageItemService, StorageItemService>();
        services.AddTransient<IFolderService, FolderService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddSession();

        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<IdentityRole>>();

        services.AddMvc();
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env
        , UserManager<User> userManager, RoleManager<IdentityRole> roleManager
        )
    {
        SeedDbInitializer.SeedRoles(roleManager);
        SeedDbInitializer.SeedUsers(Configuration, userManager);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}