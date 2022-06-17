namespace FileStorage.DAL.Entities;

public class StorageItem : BaseEntity
{
    public DateTime CreatedOn { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string RelativePath { get; set; }
    
    public int ParentFolderId { get; set; }
    public Folder ParentFolder { get; set; }
}