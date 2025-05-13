using ImportService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

public partial class ImportSyllabusPage : ContentPage
{
    private readonly ImportSyllabusPageViewModel _viewModel;

    public ImportSyllabusPage(IImportClient importClient)
    {
        InitializeComponent();
        _viewModel = new ImportSyllabusPageViewModel(importClient);
        BindingContext = _viewModel;
    }
}