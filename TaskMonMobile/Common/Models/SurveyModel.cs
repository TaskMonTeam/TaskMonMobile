namespace TaskMonMobile.Common.Models;

public class Survey
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<Module> Modules { get; set; }
}