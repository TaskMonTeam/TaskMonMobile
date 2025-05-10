using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class TimelinePage : ContentPage
{
    private TimelinePageViewModel _viewModel;
    
    public TimelinePage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new TimelinePageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }
}