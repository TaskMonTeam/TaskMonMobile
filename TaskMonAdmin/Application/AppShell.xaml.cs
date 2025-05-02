namespace TaskMonAdmin;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("SyllabusPage", typeof(SyllabusPage));
        Routing.RegisterRoute("SyllabusGroupPage", typeof(SyllabusGroupPage));
        Routing.RegisterRoute("CreateSyllabusPage", typeof(CreateSyllabusPage));
    }
}