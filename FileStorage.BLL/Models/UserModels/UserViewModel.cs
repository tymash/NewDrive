namespace FileStorage.BLL.Models.UserModels;

/// <summary>

/// The user view model class

/// </summary>

public class UserViewModel
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Gets or sets the value of the email
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Gets or sets the value of the name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the value of the surname
    /// </summary>
    public string Surname { get; set; }
    /// <summary>
    /// Gets or sets the value of the files ids
    /// </summary>
    public ICollection<int> FilesIds { get; set; }
    /// <summary>
    /// Gets or sets the value of the token
    /// </summary>
    public string Token { get; set; }
}