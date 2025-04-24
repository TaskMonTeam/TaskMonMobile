using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SurveyService.Models;

namespace TaskMonMobile.ViewModels
{
    public partial class ThemeViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private float _timeSpend;

        [ObservableProperty]
        private ObservableCollection<LessonViewModel> _lessons;

        public string ThemeTitle => $"Тема: {Title}";
        public string ThemeTimeSpendDisplay => $"({TimeSpend} годин)";

        public void RecalculateTimeSpend()
        {
            TimeSpend = Lessons.Sum(l => l.TimeSpend);
        }

        partial void OnTimeSpendChanged(float value)
        {
            OnPropertyChanged(nameof(ThemeTimeSpendDisplay));
        }
        
        public static ThemeViewModel FromModel(Theme theme, ModuleViewModel parentModule)
        {
            var viewModel = new ThemeViewModel
            {
                Id = theme.Id,
                Title = theme.Title,
                TimeSpend = 0,
                Lessons = new ObservableCollection<LessonViewModel>()
            };
        
            foreach (var lesson in theme.Lessons)
            {
                viewModel.Lessons.Add(LessonViewModel.FromModel(lesson, parentModule, viewModel));
            }
        
            return viewModel;
        }
    }
}