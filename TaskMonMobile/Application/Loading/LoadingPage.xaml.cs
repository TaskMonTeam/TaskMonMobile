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
            
            if (isAuthenticated)
            {
                await Shell.Current.GoToAsync("//SurveyGroupPage");
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