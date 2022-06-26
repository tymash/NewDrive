using FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.DAL.Context;

public class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
{
    public void Configure(EntityTypeBuilder<StorageItem> builder)
    {
        builder.HasKey(storageItem => storageItem.Id);

        builder.Property(storageItem => storageItem.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(storageItem => storageItem.IsRecycled)
            .HasDefaultValue(false);

        builder.Property(storageItem => storageItem.IsPublic)
            .HasDefaultValue(false);

        builder.Property(storageItem => storageItem.Extension)
            .HasMaxLength(20);

        builder.Property(storageItem => storageItem.RelativePath)
            .HasMaxLength(900)
            .IsRequired();
        
        builder.Property(storageItem => storageItem.CreatedOn)
            .IsRequired()
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(storageItem => storageItem.ParentFolder)
            .WithMany(folder => folder.StorageItems)
            .HasForeignKey(storageItem => storageItem.ParentFolderId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(storageItem => storageItem.User)
            .WithMany(user => user.StorageItems)
            .HasForeignKey(storageItem => storageItem.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}