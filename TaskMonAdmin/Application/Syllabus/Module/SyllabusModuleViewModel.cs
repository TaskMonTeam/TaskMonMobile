using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusModuleViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<SyllabusThemeViewModel> _themes;

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

        public static SyllabusModuleViewModel FromModel(Module module)
        {
            var viewModel = new SyllabusModuleViewModel
            {
                Title = module.Title
            };
        
            foreach (var theme in module.Themes)
            {
                viewModel.Themes.Add(SyllabusThemeViewModel.FromModel(theme));
            }
        
            return viewModel;
        }
    }
}