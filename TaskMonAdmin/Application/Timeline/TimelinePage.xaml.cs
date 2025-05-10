using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(SurveyId), "surveyId")]
public partial class TimelinePage : ContentPage
{
    private TimelinePageViewModel _viewModel;
    
    private Guid _surveyId;
    public Guid SurveyId
    {
        get => _surveyId;
        set
        {
            _surveyId = value;
            if (_viewModel != null)
            {
                _viewModel.SurveyId = value;
                _ = _viewModel.LoadSurveyData();
            }
        }
    }
    
    public TimelinePage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new TimelinePageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }
}