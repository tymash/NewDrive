namespace FileStorage.DAL.Entities;

public class Folder : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public bool IsPrimaryFolder { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public ICollection<StorageItem> StorageItems { get; set; }
}