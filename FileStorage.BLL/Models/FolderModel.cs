namespace FileStorage.BLL.Models;

public class FolderModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    
    public bool IsPrimaryFolder { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public ICollection<int> StorageItemsIds { get; set; }
}