using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ImportService.Models;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedModuleViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<ImportedThemeViewModel> _themes;
        
        [ObservableProperty]
        private bool _isExpanded = true;

        public ImportedModuleViewModel()
        {
            Themes = new ObservableCollection<ImportedThemeViewModel>();
        }
        
        public float TotalStudyHours => CalculateTotalStudyHours();
        
        public string TotalStudyHoursDisplay => $"({TotalStudyHours} годин)";
        
        private float CalculateTotalStudyHours()
        {
            float total = 0;
            foreach (var theme in Themes)
            {
                total += theme.TotalStudyHours;
            }
            return total;
        }
        
        [RelayCommand]
        private void ToggleExpanded()
        {
            IsExpanded = !IsExpanded;
        }

        public static ImportedModuleViewModel FromModel(Module module)
        {
            var viewModel = new ImportedModuleViewModel
            {
                Title = module.Title,
                IsExpanded = true
            };
        
            foreach (var theme in module.Themes)
            {
                viewModel.Themes.Add(ImportedThemeViewModel.FromModel(theme));
            }
        
            return viewModel;
        }
    }
}