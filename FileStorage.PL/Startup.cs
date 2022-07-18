using System.Text;
using AutoMapper;
using FileStorage.BLL;
using FileStorage.BLL.Mapping;
using FileStorage.BLL.Models.FileModels;
using FileStorage.BLL.Models.UserModels;
using FileStorage.BLL.Services;
using FileStorage.BLL.Services.Interfaces;
using FileStorage.BLL.Tokens;
using FileStorage.BLL.Validation.File;
using FileStorage.BLL.Validation.User;
using FileStorage.DAL.Context;
using FileStorage.DAL.Entities;
using FileStorage.DAL.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

        services.AddAuthorization();
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                }
            );
        services.AddScoped<IValidator<UserRegisterModel>, UserRegisterModelValidator>();
        services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator>();
        services.AddScoped<IValidator<UserEditModel>, UserEditModelValidator>();
        services.AddScoped<IValidator<UserChangePasswordModel>, UserChangePasswordModelValidator>();

        services.AddScoped<IValidator<FileCreateModel>, FileCreateModelValidator>();
        services.AddScoped<IValidator<FileEditModel>, FileEditModelValidator>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        
        services.AddCors(options =>
        {
            options.AddPolicy("EnableCORS", builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200");
            });
        });
        
        services.AddSwaggerGen();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env
        , UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        SeedDbInitializer.SeedRoles(roleManager);
        SeedDbInitializer.SeedUsers(userManager);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseStaticFiles();

        app.UseCors("EnableCORS");
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}