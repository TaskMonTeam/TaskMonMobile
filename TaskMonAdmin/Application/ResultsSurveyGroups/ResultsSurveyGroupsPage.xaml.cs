using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class ResultsSurveyGroupsPage : ContentPage
{
    private readonly ResultsSurveyGroupsPageViewModel _viewModel;

    public ResultsSurveyGroupsPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new ResultsSurveyGroupsPageViewModel(statisticsClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyGroupResultsAsync();
    }
}