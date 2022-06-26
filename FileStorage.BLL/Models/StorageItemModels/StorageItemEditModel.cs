namespace FileStorage.BLL.Models.StorageItemModels;

public class StorageItemEditModel
{
    public int Id { get; set; }
    public int ParentFolderId { get; set; }
    public string UserId { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string RelativePath { get; set; }
}