using FileStorage.BLL.Models.UserModels;

namespace FileStorage.BLL.Models.FileModels;

public class FileViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public UserViewModel User { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public string Extension { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public bool IsRecycled { get; set; }
    public bool IsPublic { get; set; }
    public string Path { get; set; }
}