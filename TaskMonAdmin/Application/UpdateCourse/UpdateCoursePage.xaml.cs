using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(CourseId), "courseId")]
[QueryProperty(nameof(CourseTitle), "courseTitle")]
public partial class UpdateCoursePage : ContentPage
{
    private readonly UpdateCoursePageViewModel _viewModel;

    public string CourseId { get; set; }
    public string CourseTitle { get; set; }

    public UpdateCoursePage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new UpdateCoursePageViewModel(adminClient);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(CourseId) && !string.IsNullOrEmpty(CourseTitle))
        {
            _viewModel.Initialize(Guid.Parse(CourseId), CourseTitle);
        }
    }
}