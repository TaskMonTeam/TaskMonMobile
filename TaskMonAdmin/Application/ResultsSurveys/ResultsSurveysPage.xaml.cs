using StatisticsService.Client;
using TaskMonAdmin.Services;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class ResultsSurveysPage : ContentPage
{
    private readonly ResultsSurveysPageViewModel _viewModel;
    private readonly Auth0Service _auth0Service;

    public ResultsSurveysPage(IStatisticsClient statisticsClient, Auth0Service auth0Service)
    {
        InitializeComponent();
        _viewModel = new ResultsSurveysPageViewModel(statisticsClient);
        _auth0Service = auth0Service;
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyResultsAsync();
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