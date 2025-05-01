using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(SyllabusId), "syllabusId")]
public partial class SyllabusPage : ContentPage
{
    private readonly SyllabusPageViewModel _viewModel;
    private string _syllabusId;

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

    public SyllabusPage(ITaskMonAdminClient syllabusesClient)
    {
        InitializeComponent();
        _viewModel = new SyllabusPageViewModel(syllabusesClient);
        BindingContext = _viewModel;
        
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            Command = new Command(async () => await Shell.Current.GoToAsync(".."))
        });
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadSyllabusDataAsync();
    }
}