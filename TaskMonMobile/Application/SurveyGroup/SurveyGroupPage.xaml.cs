using SurveyService.Client;
using TaskMonMobile.Services;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

[QueryProperty(nameof(GroupId), "groupId")]
public partial class SurveyGroupPage : ContentPage
{
    private readonly SurveyGroupPageViewModel _viewModel;
    private readonly Auth0Service _auth0Service;
    private string _groupId;

    public string GroupId
    {
        get => _groupId;
        set
        {
            _groupId = value;
            if (Guid.TryParse(value, out Guid groupId))
            {
                _viewModel.GroupId = groupId;
            }
        }
    }

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
        try
        {
            await _viewModel.LoadSurveyGroupDataAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", "Не вірне посилання, спробуйте знову", "OK");
            
            if (!string.IsNullOrEmpty(GroupId))
            {
                Preferences.Remove("LastOpenedGroupId");
                GroupId = string.Empty;
                _viewModel.GroupId = Guid.Empty;
            }
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool logoutResult = await _auth0Service.LogoutAsync();
        
        if (logoutResult)
        {
            Preferences.Remove("LastOpenedGroupId");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await DisplayAlert("Вийти не виходить", "Не виходить вийти з акаунту. Спробуйте знову", "OK");
        }
    }
}