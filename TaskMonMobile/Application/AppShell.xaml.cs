namespace TaskMonMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("SurveyPage", typeof(SurveyPage));
        Routing.RegisterRoute("SurveyGroupPage", typeof(SurveyGroupPage));
    }
}