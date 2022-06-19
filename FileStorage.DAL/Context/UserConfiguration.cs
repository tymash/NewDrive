using FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.DAL.Context;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
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