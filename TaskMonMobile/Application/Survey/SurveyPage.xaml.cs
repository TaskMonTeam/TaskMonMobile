using SurveyService.Client;
using TaskMonMobile.ViewModels;

namespace TaskMonMobile;

[QueryProperty(nameof(SurveyId), "surveyId")]
[QueryProperty(nameof(GroupId), "groupId")]
public partial class SurveyPage : ContentPage
{
    private readonly SurveyPageViewModel _viewModel;
    private string _surveyId;
    private string _groupId;

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