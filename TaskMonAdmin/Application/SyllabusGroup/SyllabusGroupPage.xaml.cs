using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class SyllabusGroupPage : ContentPage
{
    private readonly SyllabusGroupPageViewModel _viewModel;

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