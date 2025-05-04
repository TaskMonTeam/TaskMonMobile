using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class SurveyGroupsPage : ContentPage
{
    private readonly SurveyGroupsPageViewModel _viewModel;

    public SurveyGroupsPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new SurveyGroupsPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveyGroupsAsync();
    }
}