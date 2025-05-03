using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(CourseId), "courseId")]
public partial class SyllabusGroupPage : ContentPage
{
    private readonly SyllabusGroupPageViewModel _viewModel;
    private string _courseId;

    public string CourseId
    {
        get => _courseId;
        set
        {
            _courseId = value;
            if (Guid.TryParse(value, out Guid courseId))
            {
                _viewModel.CourseId = courseId;
            }
        }
    }

    public SyllabusGroupPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new SyllabusGroupPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSyllabusesAsync();
    }
}