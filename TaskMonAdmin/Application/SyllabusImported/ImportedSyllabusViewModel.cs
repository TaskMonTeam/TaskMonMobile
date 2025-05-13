using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ImportService.Client;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedSyllabusViewModel : ObservableObject
    {
        private readonly IImportClient _importClient;
        
        [ObservableProperty]
        private Guid _documentId;
        
        [ObservableProperty]
        private ObservableCollection<ImportedModuleViewModel> _modules;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public ImportedSyllabusViewModel(IImportClient importClient)
        {
            _importClient = importClient;
            Modules = new ObservableCollection<ImportedModuleViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadDocumentAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        public async Task LoadDocumentAsync()
        {
            try
            {
                var document = await _importClient.GetSyllabusDocument(DocumentId);
                Modules.Clear();
                
                foreach (var module in document.Modules)
                {
                    Modules.Add(ImportedModuleViewModel.FromModel(module));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}