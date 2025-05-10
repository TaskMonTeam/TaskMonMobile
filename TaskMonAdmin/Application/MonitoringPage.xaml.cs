using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class MonitoringPage : ContentPage
{
    private MonitoringPageViewModel _viewModel;
    public MonitoringPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new MonitoringPageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }

    private void CustomChip_SelectedChanged(object sender, bool isSelected)
    {
        _viewModel.UpdateChartCommand.Execute(null);
    }
}