namespace FileStorage.BLL.Models.UserModels;

/// <summary>

/// The user register model class

/// </summary>

public class UserRegisterModel
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
    /// Gets or sets the value of the email
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }
}