using Microsoft.AspNetCore.Identity;

namespace FileStorage.DAL.Entities;

/// <summary>

/// The user class

/// </summary>

/// <seealso cref="IdentityUser"/>

public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the surname
    /// </summary>
    public string Surname { get; set; }
    /// <summary>
    /// Gets or sets the value of the files
    /// </summary>
    public virtual ICollection<File> Files { get; set; }

}