using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ImportService.Client;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedSyllabusesPageViewModel : ObservableObject
    {
        private readonly IImportClient _importClient;
        
        [ObservableProperty]
        private ObservableCollection<ImportedSyllabusItemViewModel> _documents;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public ImportedSyllabusesPageViewModel(IImportClient importClient)
        {
            _importClient = importClient;
            Documents = new ObservableCollection<ImportedSyllabusItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadDocumentsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task NavigateToImport()
        {
            await Shell.Current.GoToAsync("ImportSyllabusPage");
        }
        
        public async Task LoadDocumentsAsync()
        {
            try
            {
                var documents = await _importClient.GetDocuments();
                Documents.Clear();
                
                foreach (var document in documents)
                {
                    var viewModel = ImportedSyllabusItemViewModel.FromModel(document);
                    Documents.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}