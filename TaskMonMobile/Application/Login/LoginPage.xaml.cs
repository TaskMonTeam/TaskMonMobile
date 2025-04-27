using TaskMonMobile.Services;

namespace TaskMonMobile;

public partial class LoginPage : ContentPage
{
    private readonly Auth0Service _auth0Service;

    public LoginPage(Auth0Service auth0Service)
    {
        InitializeComponent();
        _auth0Service = auth0Service;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            bool loginResult = await _auth0Service.LoginAsync();
            
            if (loginResult)
            {
                await Shell.Current.GoToAsync("//SurveyGroupPage");
            }
            else
            {
                await DisplayAlert("Логін не пройшов", "Не вийшло залогінитися. Спробуйте знову", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", $"З'явилась наступна помилка: {ex.Message}", "OK");
        }
    }
}