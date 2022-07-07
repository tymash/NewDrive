namespace FileStorage.BLL.Models.FolderModels;

public class FolderEditModel
{
    public int Id { get; set; }
    public bool IsPrimaryFolder { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}