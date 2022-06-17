using FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.DAL.Context;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.HasKey(folder => folder.Id);
        
        builder.HasOne(folder => folder.User)
            .WithMany(user => user.Folders)
            .HasForeignKey(folder => folder.UserId);
    }
}