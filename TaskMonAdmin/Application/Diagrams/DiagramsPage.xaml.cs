using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(SurveyId), "surveyId")]
public partial class DiagramsPage : ContentPage
{
    private DiagramsPageViewModel _viewModel;
    
    private Guid _surveyId;
    public Guid SurveyId
    {
        get => _surveyId;
        set
        {
            _surveyId = value;
            _viewModel.SurveyId = value;
            _ = _viewModel.LoadSurveyData();
        }
    }
    
    public DiagramsPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new DiagramsPageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }
}