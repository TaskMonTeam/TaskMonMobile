using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class TimelineGroupPage : ContentPage
{
    private TimelineGroupPageViewModel _viewModel;
    public TimelineGroupPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new TimelineGroupPageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }

    private void CustomChip_SelectedChanged(object sender, bool isSelected)
    {
        _viewModel.UpdateChartCommand.Execute(null);
    }
}