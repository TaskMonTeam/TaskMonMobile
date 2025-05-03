using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class CoursesPage : ContentPage
{
    private readonly CoursesPageViewModel _viewModel;

    public CoursesPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CoursesPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadCoursesAsync();
    }
}