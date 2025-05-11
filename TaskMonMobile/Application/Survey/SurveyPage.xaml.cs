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
    
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        
        if (Guid.TryParse(_surveyId, out Guid surveyId))
        {
            string key = $"CompletedSurvey_{_viewModel.GroupId}_{surveyId}";
            bool isCompleted = Preferences.Get(key, false);
            
            if (isCompleted)
            {
                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2];
                    if (previousPage is SurveyGroupPage surveyGroupPage && 
                        surveyGroupPage.BindingContext is SurveyGroupPageViewModel viewModel)
                    {
                        viewModel.MarkSurveyAsCompleted(surveyId);
                    }
                }
            }
        }
    }
}