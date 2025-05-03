using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class CreateCoursePage : ContentPage
{
    private readonly CreateCoursePageViewModel _viewModel;

    public CreateCoursePage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CreateCoursePageViewModel(adminClient);
        BindingContext = _viewModel;
        
        SetupBackButtonBehavior();
    }
    
    private void SetupBackButtonBehavior()
    {
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            Command = new Command(async () => await Shell.Current.GoToAsync(".."))
        });
    }
}