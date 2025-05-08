using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Models;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusModuleViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<SyllabusThemeViewModel> _themes;
        
        [ObservableProperty]
        private bool _isExpanded = true;

        public SyllabusModuleViewModel()
        {
            Themes = new ObservableCollection<SyllabusThemeViewModel>();
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

        public static SyllabusModuleViewModel FromModel(Module module)
        {
            var viewModel = new SyllabusModuleViewModel
            {
                Title = module.Title,
                IsExpanded = true
            };
        
            foreach (var theme in module.Themes)
            {
                viewModel.Themes.Add(SyllabusThemeViewModel.FromModel(theme));
            }
        
            return viewModel;
        }
    }
}