using ImportService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class ImportedSyllabusesPage : ContentPage
{
    private readonly ImportedSyllabusesPageViewModel _viewModel;

    public ImportedSyllabusesPage(IImportClient importClient)
    {
        InitializeComponent();
        _viewModel = new ImportedSyllabusesPageViewModel(importClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadDocumentsAsync();
    }
}