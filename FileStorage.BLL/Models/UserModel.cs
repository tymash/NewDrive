namespace FileStorage.BLL.Models;

public class UserModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    
    public ICollection<int> FoldersIds { get; set; }
    public ICollection<int> StorageItemsIds { get; set; }
}