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
        private float _timeSpend;

        [ObservableProperty]
        private string _lessonTimeSpendText;

        public LessonViewModel(ModuleViewModel parentModule, ThemeViewModel parentTheme)
        {
            _parentModule = parentModule;
            _parentTheme = parentTheme;
        }

        public string LessonTypes => $"{Type}";
        public string LessonTimeSpendDisplay => $"Витрачений час: {TimeSpend}";

        partial void OnTimeSpendChanged(float value)
        {
            LessonTimeSpendText = value > 0 ? value.ToString() : string.Empty;
            OnPropertyChanged(nameof(LessonTimeSpendDisplay));
            UpdateParentTimeSpend();
        }

        partial void OnLessonTimeSpendTextChanged(string value)
        {
            float timeSpend = 0;
    
            if (string.IsNullOrWhiteSpace(value))
            {
                if (TimeSpend != 0)
                {
                    TimeSpend = 0;
                }
            }
            else if (float.TryParse(value, out timeSpend))
            {
                if (TimeSpend != timeSpend)
                {
                    TimeSpend = timeSpend;
                }
            }
        }

        partial void OnTitleChanged(string value)
        {
            OnPropertyChanged(nameof(LessonTypes));
        }

        partial void OnTypeChanged(LessonType value)
        {
            OnPropertyChanged(nameof(LessonTypes));
        }

        private void UpdateParentTimeSpend()
        {
            _parentTheme.RecalculateTimeSpend();
            _parentModule.TimeSpend = _parentModule.Themes.Sum(t => t.TimeSpend);
        }
        
        public static LessonViewModel FromModel(Lesson lesson, ModuleViewModel parentModule, ThemeViewModel parentTheme)
        {
            return new LessonViewModel(parentModule, parentTheme)
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Type = lesson.Type,
                TimeSpend = 0
            };
        }
    }
}