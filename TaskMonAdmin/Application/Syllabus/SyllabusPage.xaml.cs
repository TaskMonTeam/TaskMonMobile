using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(SyllabusId), "syllabusId")]
[QueryProperty(nameof(CourseId), "courseId")]
[QueryProperty(nameof(UseCurrentSyllabus), "useCurrentSyllabus")]
public partial class SyllabusPage : ContentPage
{
    private readonly SyllabusPageViewModel _viewModel;
    private string _syllabusId;
    private string _courseId;
    private string _useCurrentSyllabus;

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
    
    public string UseCurrentSyllabus
    {
        get => _useCurrentSyllabus;
        set
        {
            _useCurrentSyllabus = value;
            _viewModel.UseCurrentSyllabus = !string.IsNullOrEmpty(value) && 
                                            value.Equals("true", StringComparison.OrdinalIgnoreCase);
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