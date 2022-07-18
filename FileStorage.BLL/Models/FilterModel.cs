namespace FileStorage.BLL.Models;

/// <summary>

/// The filter model class

/// </summary>

public class FilterModel
{
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the date sort
    /// </summary>
    public Sort? DateSort { get; set; }
    /// <summary>
    /// Gets or sets the value of the name sort
    /// </summary>
    public Sort? NameSort { get; set; }
    /// <summary>
    /// Gets or sets the value of the size sort
    /// </summary>
    public Sort? SizeSort { get; set; }
    /// <summary>
    /// Gets or sets the value of the is recycled
    /// </summary>
    public bool? IsRecycled { get; set; }
    /// <summary>
    /// Gets or sets the value of the is public
    /// </summary>
    public bool? IsPublic { get; set; }
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string? UserId { get; set; }
}