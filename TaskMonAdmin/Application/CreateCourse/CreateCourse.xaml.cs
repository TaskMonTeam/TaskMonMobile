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
    }
}