using ImportService.Client;
using TaskMonAdmin.ViewModels;

namespace TaskMonAdmin;

[QueryProperty(nameof(DocumentId), "documentId")]
public partial class SyllabusImportedPage : ContentPage
{
    private readonly ImportedSyllabusViewModel _viewModel;
    private string _documentId;

    public string DocumentId
    {
        get => _documentId;
        set
        {
            _documentId = value;
            if (Guid.TryParse(value, out Guid documentId))
            {
                _viewModel.DocumentId = documentId;
            }
        }
    }

    public SyllabusImportedPage(IImportClient importClient)
    {
        InitializeComponent();
        _viewModel = new ImportedSyllabusViewModel(importClient);
        BindingContext = _viewModel;
        
        Loaded += OnPageLoaded;
    }
    
    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await _viewModel.LoadDocumentAsync();
    }
}