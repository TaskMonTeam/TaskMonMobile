using AdminService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class CreateSyllabusPage : ContentPage
{
    private readonly CreateSyllabusPageViewModel _viewModel;

    public CreateSyllabusPage(ITaskMonAdminClient adminClient)
    {
        InitializeComponent();
        _viewModel = new CreateSyllabusPageViewModel(adminClient);
        BindingContext = _viewModel;
        
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            Command = new Command(async () => await Shell.Current.GoToAsync(".."))
        });
        
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