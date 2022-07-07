namespace FileStorage.BLL.Models.FolderModels;

public class FolderViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public bool IsPrimaryFolder { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Path { get; set; }
    
    public ICollection<int> FilesIds { get; set; }
}