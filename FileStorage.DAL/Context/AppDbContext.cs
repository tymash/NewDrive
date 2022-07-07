using FileStorage.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Context;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }

    public AppDbContext()
    {
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=fileStorage;User Id=sa;Password=Tymash33;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new FileConfiguration());
        builder.ApplyConfiguration(new FolderConfiguration());
    }
}