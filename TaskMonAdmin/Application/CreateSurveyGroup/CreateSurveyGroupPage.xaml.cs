using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class CreateSurveyGroupPage : ContentPage
{
    private readonly CreateSurveyGroupPageViewModel _viewModel;

    public CreateSurveyGroupPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CreateSurveyGroupPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSurveysAsync();
    }
}