namespace TaskMonAdmin;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("CreateCoursePage", typeof(CreateCoursePage));
        Routing.RegisterRoute("SyllabusGroupPage", typeof(SyllabusGroupPage));
        Routing.RegisterRoute("SyllabusPage", typeof(SyllabusPage));
        Routing.RegisterRoute("CreateSyllabusPage", typeof(CreateSyllabusPage));
        Routing.RegisterRoute("UpdateCoursePage", typeof(UpdateCoursePage));
        Routing.RegisterRoute("CreateSurveyGroupPage", typeof(CreateSurveyGroupPage));
        Routing.RegisterRoute("CreateSurveyPage", typeof(CreateSurveyPage));
        Routing.RegisterRoute("LinkPage", typeof(LinkPage));
        Routing.RegisterRoute("DiagramsPage", typeof(DiagramsPage));
        Routing.RegisterRoute("DiagramsGroupPage", typeof(DiagramsGroupPage));
        Routing.RegisterRoute("ImportSyllabusPage", typeof(ImportSyllabusPage));
        Routing.RegisterRoute("ImportedSyllabusesPage", typeof(ImportedSyllabusesPage));
        Routing.RegisterRoute("SyllabusImportedPage", typeof(SyllabusImportedPage));
    }
}