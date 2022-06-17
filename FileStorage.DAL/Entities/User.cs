using Microsoft.AspNetCore.Identity;

namespace FileStorage.DAL.Entities;

public class User : IdentityUser
{
    public ICollection<Folder> Folders { get; set; }
    
}