using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SurveyService.Models;

namespace TaskMonMobile.ViewModels
{
    public partial class ModuleViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private float _rating;

        [ObservableProperty]
        private ObservableCollection<ThemeViewModel> _themes;

        public string ModuleTitle => $"Модуль: {Title}";
        public string ModuleRatingDisplay => $"Оцінка: {Rating}";

        partial void OnRatingChanged(float value)
        {
            OnPropertyChanged(nameof(ModuleRatingDisplay));
        }
        
        public static ModuleViewModel FromModel(Module module)
        {
            var viewModel = new ModuleViewModel
            {
                Id = module.Id,
                Title = module.Title,
                Rating = 0,
                Themes = new ObservableCollection<ThemeViewModel>()
            };
        
            foreach (var theme in module.Themes)
            {
                viewModel.Themes.Add(ThemeViewModel.FromModel(theme, viewModel));
            }
        
            return viewModel;
        }
    }
}