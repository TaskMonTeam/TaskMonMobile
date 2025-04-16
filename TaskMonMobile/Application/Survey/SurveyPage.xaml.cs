using SurveyService.Client;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

public partial class SurveyPage : ContentPage
{
    private readonly SurveyPageViewModel _viewModel;

    public SurveyPage(ISurveyClient surveyClient)
    {
        InitializeComponent();
        _viewModel = new SurveyPageViewModel(surveyClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyDataAsync();
    }
    
    /*private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }*/
}