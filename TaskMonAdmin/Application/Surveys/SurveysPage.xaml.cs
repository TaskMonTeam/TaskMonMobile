using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class SurveysPage : ContentPage
{
    private readonly SurveysPageViewModel _viewModel;

    public SurveysPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new SurveysPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveysAsync();
    }
}