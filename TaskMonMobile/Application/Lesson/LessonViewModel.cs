using CommunityToolkit.Mvvm.ComponentModel;
using SurveyService.Models;

namespace TaskMonMobile.ViewModels
{
    public partial class LessonViewModel : ObservableObject
    {
        private readonly ModuleViewModel _parentModule;
        private readonly ThemeViewModel _parentTheme;

        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private LessonType _type;

        [ObservableProperty]
        private float _rating;

        [ObservableProperty]
        private string _lessonRatingText;

        public LessonViewModel(ModuleViewModel parentModule, ThemeViewModel parentTheme)
        {
            _parentModule = parentModule;
            _parentTheme = parentTheme;
        }

        public string LessonTitleWithType => $"Завдання: {Title} (Тип: {Type})";
        public string LessonRatingDisplay => $"Оцінка: {Rating}";

        partial void OnRatingChanged(float value)
        {
            LessonRatingText = value > 0 ? value.ToString() : string.Empty;
            OnPropertyChanged(nameof(LessonRatingDisplay));
            UpdateParentRatings();
        }

        partial void OnLessonRatingTextChanged(string value)
        {
            float rating = 0;
    
            if (string.IsNullOrWhiteSpace(value))
            {
                if (Rating != 0)
                {
                    Rating = 0;
                }
            }
            else if (float.TryParse(value, out rating))
            {
                if (Rating != rating)
                {
                    Rating = rating;
                }
            }
        }

        partial void OnTitleChanged(string value)
        {
            OnPropertyChanged(nameof(LessonTitleWithType));
        }

        partial void OnTypeChanged(LessonType value)
        {
            OnPropertyChanged(nameof(LessonTitleWithType));
        }

        private void UpdateParentRatings()
        {
            _parentTheme.RecalculateRating();
            _parentModule.Rating = _parentModule.Themes.Sum(t => t.Rating);
        }
        
        public static LessonViewModel FromModel(Lesson lesson, ModuleViewModel parentModule, ThemeViewModel parentTheme)
        {
            return new LessonViewModel(parentModule, parentTheme)
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Type = lesson.Type,
                Rating = 0
            };
        }
    }
}