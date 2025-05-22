using AdminService.Client;
using TaskMonAdmin.Services;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class SurveyGroupsPage : ContentPage
{
    private readonly SurveyGroupsPageViewModel _viewModel;
    private readonly Auth0Service _auth0Service;

    public SurveyGroupsPage(ITaskMonAdminClient adminClient, Auth0Service auth0Service)
    {
        InitializeComponent();
        _viewModel = new SurveyGroupsPageViewModel(adminClient);
        _auth0Service = auth0Service;
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyGroupsAsync();
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