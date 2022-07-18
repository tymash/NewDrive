using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Context;

/// <summary>

/// The app db context class

/// </summary>

/// <seealso cref="IdentityDbContext{User}"/>

public class AppDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Gets or sets the value of the users
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Gets or sets the value of the files
    /// </summary>
    public DbSet<File> Files { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class
    /// </summary>
    public AppDbContext()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class
    /// </summary>
    /// <param name="options">The options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Ons the configuring using the specified options builder
    /// </summary>
    /// <param name="optionsBuilder">The options builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=fileStorage;User Id=sa;Password=Tymash33;");
    }

    /// <summary>
    /// Ons the model creating using the specified builder
    /// </summary>
    /// <param name="builder">The builder</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());
    }
}