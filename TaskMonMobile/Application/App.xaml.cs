using CommunityToolkit.Mvvm.Messaging;
using TaskMonMobile.Services;

namespace TaskMonMobile;

public partial class App : Application
{
    private readonly Auth0Service _auth0Service;
    
    public App(Auth0Service auth0Service)
    {
        InitializeComponent();
        _auth0Service = auth0Service;
        
        WeakReferenceMessenger.Default.Register<DeepLinkMessage>(this, async (r, m) =>
        {
            var (type, id) = m.Value;
            await HandleDeepLink(type, id);
        });
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
    
    private async Task HandleDeepLink(DeepLinkType type, string id)
    {
        try
        {
            bool isAuthenticated = await _auth0Service.IsAuthenticatedAsync();
            
            if (!isAuthenticated)
            {
                Preferences.Set("PendingDeepLinkType", (int)type);
                Preferences.Set("PendingDeepLinkId", id);
                
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }
            
            switch (type)
            {
                case DeepLinkType.Survey:
                    await Shell.Current.GoToAsync($"//SurveyPage?surveyId={id}");
                    break;
                case DeepLinkType.Group:
                    await Shell.Current.GoToAsync($"//SurveyGroupPage?groupId={id}");
                    break;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Помилка", $"Помилка при обробці посилання: {ex.Message}", "OK");
            await Shell.Current.GoToAsync("//SurveyGroupPage");
        }
    }
}