using FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = FileStorage.DAL.Entities.File;

namespace FileStorage.DAL.Context;

/// <summary>

/// The file configuration class

/// </summary>

/// <seealso cref="IEntityTypeConfiguration{File}"/>

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    /// <summary>
    /// Configures the builder
    /// </summary>
    /// <param name="builder">The builder</param>
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(file => file.Id);

        builder.Property(file => file.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(file => file.IsRecycled)
            .HasDefaultValue(false);

        builder.Property(file => file.IsPublic)
            .HasDefaultValue(false);

        builder.Property(file => file.Extension)
            .HasMaxLength(20);

        builder.Property(file => file.Path)
            .HasMaxLength(900)
            .IsRequired();
        
        builder.Property(file => file.CreatedOn)
            .IsRequired()
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(file => file.User)
            .WithMany(user => user.Files)
            .HasForeignKey(file => file.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}