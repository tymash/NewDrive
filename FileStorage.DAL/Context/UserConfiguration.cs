using FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.DAL.Context;

/// <summary>

/// The user configuration class

/// </summary>

/// <seealso cref="IEntityTypeConfiguration{User}"/>

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the builder
    /// </summary>
    /// <param name="builder">The builder</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(user => user.Surname)
            .HasMaxLength(50)
            .IsRequired();
    }
}