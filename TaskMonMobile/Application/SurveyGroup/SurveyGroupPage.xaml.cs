using SurveyService.Client;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

public partial class SurveyGroupPage : ContentPage
{
    private readonly SurveyGroupPageViewModel _viewModel;

    public SurveyGroupPage(ISurveyClient surveyClient)
    {
        InitializeComponent();
        _viewModel = new SurveyGroupPageViewModel(surveyClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyGroupDataAsync();
    }
}