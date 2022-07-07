namespace FileStorage.BLL.Models.FileModels;

public class FileCreateModel
{
    public string UserId { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string Path { get; set; }
}