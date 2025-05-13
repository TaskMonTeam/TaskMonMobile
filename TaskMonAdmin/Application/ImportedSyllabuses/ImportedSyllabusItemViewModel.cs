using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedSyllabusItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _documentId;

        [ObservableProperty]
        private DocumentStatus _status;
        
        [ObservableProperty]
        private string _originalFilename;
        
        public string StatusText => Status.ToString();
        
        public bool IsProcessed => Status == DocumentStatus.Processed;
        
        [RelayCommand]
        private async Task NavigateToDetails()
        {
            if (IsProcessed)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "documentId", DocumentId.ToString() }
                };
                await Shell.Current.GoToAsync("SyllabusImportedPage", navigationParameter);
            }
        }
        
        public override string ToString()
        {
            return OriginalFilename;
        }
        
        public static ImportedSyllabusItemViewModel FromModel(DocumentInfo document)
        {
            return new ImportedSyllabusItemViewModel
            {
                DocumentId = document.DocumentId,
                Status = document.Status,
                OriginalFilename = document.OriginalFilename
            };
        }
    }
}