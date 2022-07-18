namespace FileStorage.DAL.Entities;

/// <summary>

/// The file class

/// </summary>

/// <seealso cref="BaseEntity"/>

public class File : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the created on
    /// </summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>
    /// Gets or sets the value of the extension
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the size
    /// </summary>
    public long Size { get; set; }
    /// <summary>
    /// Gets or sets the value of the is recycled
    /// </summary>
    public bool IsRecycled { get; set; }
    /// <summary>
    /// Gets or sets the value of the is public
    /// </summary>
    public bool IsPublic { get; set; }
    /// <summary>
    /// Gets or sets the value of the path
    /// </summary>
    public string Path { get; set; }
    
    
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// Gets or sets the value of the user
    /// </summary>
    public virtual User User { get; set; }
}