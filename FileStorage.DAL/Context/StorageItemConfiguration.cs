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
            .HasMaxLength(300)
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

        builder.HasOne(storageItem => storageItem.ParentFolder)
            .WithMany(folder => folder.StorageItems)
            .HasForeignKey(storageItem => storageItem.ParentFolderId);

    }
}