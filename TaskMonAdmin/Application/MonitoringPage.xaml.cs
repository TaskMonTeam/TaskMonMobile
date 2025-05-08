using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class MonitoringPage : ContentPage
{
    public MonitoringPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        BindingContext = new MonitoringPageViewModel(statisticsClient);
    }
}