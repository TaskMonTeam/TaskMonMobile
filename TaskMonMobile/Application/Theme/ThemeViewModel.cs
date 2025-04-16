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
        private int _rating;

        [ObservableProperty]
        private ObservableCollection<LessonViewModel> _lessons;

        public string ThemeTitle => $"Тема: {Title}";
        public string ThemeRatingDisplay => $"Оцінка: {Rating}";

        public void RecalculateRating()
        {
            Rating = Lessons.Sum(l => l.Rating);
        }

        partial void OnRatingChanged(int value)
        {
            OnPropertyChanged(nameof(ThemeRatingDisplay));
        }
        
        public static ThemeViewModel FromModel(Theme theme, ModuleViewModel parentModule)
        {
            var viewModel = new ThemeViewModel
            {
                Id = theme.Id,
                Title = theme.Title,
                Rating = 0,
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