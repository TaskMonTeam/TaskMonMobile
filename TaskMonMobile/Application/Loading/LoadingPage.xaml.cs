using TaskMonMobile.Services;

namespace TaskMonMobile;

public partial class LoadingPage : ContentPage
{
    private readonly Auth0Service _auth0Service;

    public LoadingPage(Auth0Service auth0Service)
    {
        InitializeComponent();
        _auth0Service = auth0Service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            bool isAuthenticated = await _auth0Service.IsAuthenticatedAsync();
            
            bool hasPendingDeepLink = Preferences.Get("HasPendingDeepLink", false);
            
            if (isAuthenticated)
            {
                if (hasPendingDeepLink)
                {
                    int pendingLinkType = Preferences.Get("PendingDeepLinkType", -1);
                    string pendingLinkId = Preferences.Get("PendingDeepLinkId", string.Empty);
                    
                    Preferences.Remove("HasPendingDeepLink");
                    Preferences.Remove("PendingDeepLinkType");
                    Preferences.Remove("PendingDeepLinkId");
                    
                    if (pendingLinkType != -1 && !string.IsNullOrEmpty(pendingLinkId))
                    {
                        switch ((DeepLinkType)pendingLinkType)
                        {
                            case DeepLinkType.Survey:
                                await Shell.Current.GoToAsync($"//SurveyPage?surveyId={pendingLinkId}");
                                break;
                            case DeepLinkType.Group:
                                Preferences.Set("LastOpenedGroupId", pendingLinkId);
                                await Shell.Current.GoToAsync($"//SurveyGroupPage?groupId={pendingLinkId}");
                                break;
                            default:
                                await Shell.Current.GoToAsync("//SurveyGroupPage");
                                break;
                        }
                        return;
                    }
                }
                
                string lastGroupId = Preferences.Get("LastOpenedGroupId", string.Empty);
                if (!string.IsNullOrEmpty(lastGroupId) && Guid.TryParse(lastGroupId, out _))
                {
                    await Shell.Current.GoToAsync($"//SurveyGroupPage?groupId={lastGroupId}");
                }
                else
                {
                    await Shell.Current.GoToAsync("//SurveyGroupPage");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", $"Помилка при перевірці автентифікації: {ex.Message}", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}