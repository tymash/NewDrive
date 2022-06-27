namespace FileStorage.BLL.Models;

public class FilterModel
{
    public string? Name { get; set; }
    public Sort? DateSort { get; set; }
    public Sort? NameSort { get; set; }
    public Sort? SizeSort { get; set; }
}