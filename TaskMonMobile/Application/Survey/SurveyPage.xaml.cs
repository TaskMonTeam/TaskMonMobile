using SurveyService.Client;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

[QueryProperty(nameof(SurveyId), "surveyId")]
public partial class SurveyPage : ContentPage
{
    private readonly SurveyPageViewModel _viewModel;
    private string _surveyId;

    public string SurveyId
    {
        get => _surveyId;
        set
        {
            _surveyId = value;
            if (Guid.TryParse(value, out Guid surveyId))
            {
                _viewModel.Id = surveyId;
            }
        }
    }

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
}