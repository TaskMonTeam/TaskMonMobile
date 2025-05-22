using TaskMonAdmin.Services;

namespace TaskMonAdmin;

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
        await CheckAuthenticationStatus();
    }

    private async Task CheckAuthenticationStatus()
    {
        try
        {
            bool isAuthenticated = await _auth0Service.IsAuthenticatedAsync();

            if (isAuthenticated)
            {
                await Shell.Current.GoToAsync("//CoursesPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}