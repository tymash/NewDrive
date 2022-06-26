namespace FileStorage.BLL.Models.StorageItemModels;

public class StorageItemCreateModel
{
    public string UserId { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string RelativePath { get; set; }
}