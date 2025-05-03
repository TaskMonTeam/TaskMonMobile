namespace TaskMonAdmin;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("CreateCoursePage", typeof(CreateCoursePage));
        Routing.RegisterRoute("SyllabusGroupPage", typeof(SyllabusGroupPage));
        Routing.RegisterRoute("SyllabusPage", typeof(SyllabusPage));
        Routing.RegisterRoute("CreateSyllabusPage", typeof(CreateSyllabusPage));
        Routing.RegisterRoute("UpdateCoursePage", typeof(UpdateCoursePage));
    }
}