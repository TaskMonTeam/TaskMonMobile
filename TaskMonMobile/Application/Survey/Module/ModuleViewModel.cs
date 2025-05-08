using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private float _timeSpend;

        [ObservableProperty]
        private ObservableCollection<ThemeViewModel> _themes;
        
        [ObservableProperty]
        private bool _isExpanded = true;

        public string ModuleTitle => $"Модуль: {Title}";
        public string ModuleTimeSpendDisplay => $"({TimeSpend} годин)";

        partial void OnTimeSpendChanged(float value)
        {
            OnPropertyChanged(nameof(ModuleTimeSpendDisplay));
        }
        
        [RelayCommand]
        private void ToggleExpanded()
        {
            IsExpanded = !IsExpanded;
        }
        
        public static ModuleViewModel FromModel(Module module)
        {
            var viewModel = new ModuleViewModel
            {
                Id = module.Id,
                Title = module.Title,
                TimeSpend = 0,
                Themes = new ObservableCollection<ThemeViewModel>(),
                IsExpanded = true
            };
        
            foreach (var theme in module.Themes)
            {
                viewModel.Themes.Add(ThemeViewModel.FromModel(theme, viewModel));
            }
        
            return viewModel;
        }
    }
}