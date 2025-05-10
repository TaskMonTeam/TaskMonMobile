using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class ResultsSurveysPage : ContentPage
{
    private readonly ResultsSurveysPageViewModel _viewModel;

    public ResultsSurveysPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new ResultsSurveysPageViewModel(statisticsClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyResultsAsync();
    }
}