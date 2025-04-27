using SurveyService.Client;
using TaskMonMobile.Services;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

public partial class SurveyGroupPage : ContentPage
{
    private readonly SurveyGroupPageViewModel _viewModel;
        private readonly Auth0Service _auth0Service;

    public SurveyGroupPage(ISurveyClient surveyClient, Auth0Service auth0Service)
    {
        InitializeComponent();
        _viewModel = new SurveyGroupPageViewModel(surveyClient);
        _auth0Service = auth0Service;
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyGroupDataAsync();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool logoutResult = await _auth0Service.LogoutAsync();
        
        if (logoutResult)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await DisplayAlert("Вийти не виходить", "Не виходить вийти з акаунту. Спробуйте знову", "OK");
        }
    }
}