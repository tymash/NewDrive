using Microsoft.AspNetCore.Identity;

namespace FileStorage.DAL.Entities;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual ICollection<Folder> Folders { get; set; }
    public virtual ICollection<StorageItem> StorageItems { get; set; }

}