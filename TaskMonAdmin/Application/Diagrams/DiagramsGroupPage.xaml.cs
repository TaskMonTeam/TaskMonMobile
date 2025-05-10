using StatisticsService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(GroupId), "groupId")]
public partial class DiagramsGroupPage : ContentPage
{
    private DiagramsGroupPageViewModel _viewModel;
    
    private Guid _groupId;
    public Guid GroupId
    {
        get => _groupId;
        set
        {
            _groupId = value;
            _viewModel.GroupId = value;
            _ = _viewModel.LoadSurveyData();
        }
    }
    
    public DiagramsGroupPage(IStatisticsClient statisticsClient)
    {
        InitializeComponent();
        _viewModel = new DiagramsGroupPageViewModel(statisticsClient);
        BindingContext = _viewModel;
    }

    private void CustomChip_SelectedChanged(object sender, bool isSelected)
    {
        _viewModel.UpdateChartCommand.Execute(null);
    }
}