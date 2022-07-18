namespace FileStorage.BLL.Models.FileModels;

/// <summary>

/// The file edit model class

/// </summary>

public class FileEditModel
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the value of the extension
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string Name { get; set; }
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
}