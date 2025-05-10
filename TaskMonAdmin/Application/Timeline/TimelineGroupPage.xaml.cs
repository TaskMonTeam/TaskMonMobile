using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(GroupId), "groupId")]
public partial class TimelineGroupPage : ContentPage
{
    private TimelineGroupPageViewModel _viewModel;
    
    private Guid _groupId;
    public Guid GroupId
    {
        get => _groupId;
        set
        {
            _groupId = value;
            if (_viewModel != null)
            {
                _viewModel.GroupId = value;
                _ = _viewModel.LoadSurveyData();
            }
        }
    }
    
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