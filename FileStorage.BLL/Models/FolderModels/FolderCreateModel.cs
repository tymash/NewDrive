namespace FileStorage.BLL.Models.FolderModels;

public class FolderCreateModel
{
    public string UserId { get; set; }
    public bool IsPrimaryFolder { get; set; }
    public string Name { get; set; }
    public string RelativePath { get; set; }
}