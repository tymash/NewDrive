namespace FileStorage.BLL.Models;

public class FilterModel
{
    public string? Name { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}