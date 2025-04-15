namespace TaskMonMobile.Common.Models;

public class Module
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<Theme> Themes { get; set; }
    public int Rating { get; set; }
}