namespace TaskMonMobile.Common.Models;

public class Theme
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<Lesson> Lessons { get; set; }
    public int Rating { get; set; }
}