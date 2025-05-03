using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(SyllabusId), "syllabusId")]
[QueryProperty(nameof(CourseId), "courseId")]
public partial class SyllabusPage : ContentPage
{
    private readonly SyllabusPageViewModel _viewModel;
    private string _syllabusId;
    private string _courseId;

    public string SyllabusId
    {
        get => _syllabusId;
        set
        {
            _syllabusId = value;
            if (Guid.TryParse(value, out Guid syllabusId))
            {
                _viewModel.Id = syllabusId;
            }
        }
    }
    
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

    public SyllabusPage(ITaskMonAdminClient syllabusesClient)
    {
        InitializeComponent();
        _viewModel = new SyllabusPageViewModel(syllabusesClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSyllabusDataAsync();
    }
}