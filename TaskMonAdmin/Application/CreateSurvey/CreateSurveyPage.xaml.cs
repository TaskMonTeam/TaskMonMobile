using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class CreateSurveyPage : ContentPage
{
    private readonly CreateSurveyPageViewModel _viewModel;

    public CreateSurveyPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CreateSurveyPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadCoursesAsync();
    }
}