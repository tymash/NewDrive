namespace FileStorage.BLL.Models.FileModels;

public class FileEditModel
{
    public int Id { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string Path { get; set; }
}