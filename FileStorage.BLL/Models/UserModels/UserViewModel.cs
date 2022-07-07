namespace FileStorage.BLL.Models.UserModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public ICollection<int> FoldersIds { get; set; }
    public ICollection<int> FilesIds { get; set; }
    public string Token { get; set; }
}