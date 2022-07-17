namespace FileStorage.DAL.Entities;

public class File : BaseEntity
{
    public DateTime CreatedOn { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string Path { get; set; }
    
    
    public string UserId { get; set; }
    public virtual User User { get; set; }
}