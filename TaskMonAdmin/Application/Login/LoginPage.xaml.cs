using TaskMonAdmin.Services;

namespace TaskMonAdmin;

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
            bool loginSuccess = await _auth0Service.LoginAsync();

            if (loginSuccess)
            {
                await Shell.Current.GoToAsync("//CoursesPage");
            }
            else
            {
                await DisplayAlert("Помилка", "Не вдалося увійти в аккаунт. Спробуйте ще раз.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", "Сталася помилка під час входу в аккаунт.", "OK");
        }
    }
}