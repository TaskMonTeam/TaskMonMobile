using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(CourseId), "courseId")]
public partial class CreateSyllabusPage : ContentPage
{
    private readonly CreateSyllabusPageViewModel _viewModel;
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

    public CreateSyllabusPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CreateSyllabusPageViewModel(adminClient);
        BindingContext = _viewModel;
    
        Loaded += OnPageLoaded;
    }
    
    private void OnPageLoaded(object sender, EventArgs e)
    {
        if (_viewModel.Modules.Count == 0)
        {
            _viewModel.AddModuleCommand.Execute(null);
        }
    }
}