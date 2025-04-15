using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskMonMobile.Common.Models;

namespace TaskMonMobile.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly ISurveyClient _surveyClient;
        private SurveyViewModel _currentSurvey;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public SurveyViewModel CurrentSurvey
        {
            get => _currentSurvey;
            set
            {
                if (_currentSurvey != value)
                {
                    _currentSurvey = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
        }
        
        public MainPageViewModel()
        {
            _surveyClient = null;
        }

        public async Task LoadSurveyDataAsync()
        {
            try
            {
                var survey = await _surveyClient.GetSurveyByIdAsync("1e8d6dc3-38ee-4379-9855-08c639962663");
                CurrentSurvey = ConvertToViewModel(survey);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }

        private SurveyViewModel ConvertToViewModel(Survey survey)
        {
            var viewModel = new SurveyViewModel
            {
                Id = survey.Id,
                Title = survey.Title,
                Modules = new ObservableCollection<ModuleViewModel>()
            };

            foreach (var module in survey.Modules)
            {
                var moduleViewModel = new ModuleViewModel
                {
                    Id = module.Id,
                    Title = module.Title,
                    Rating = module.Rating,
                    Themes = new ObservableCollection<ThemeViewModel>()
                };

                foreach (var theme in module.Themes)
                {
                    var themeViewModel = new ThemeViewModel
                    {
                        Id = theme.Id,
                        Title = theme.Title,
                        Rating = theme.Rating,
                        Lessons = new ObservableCollection<LessonViewModel>()
                    };

                    foreach (var lesson in theme.Lessons)
                    {
                        var lessonViewModel = new LessonViewModel(moduleViewModel, themeViewModel)
                        {
                            Id = lesson.Id,
                            Title = lesson.Title,
                            Type = lesson.Type,
                            Rating = lesson.Rating
                        };

                        themeViewModel.Lessons.Add(lessonViewModel);
                    }

                    moduleViewModel.Themes.Add(themeViewModel);
                }

                viewModel.Modules.Add(moduleViewModel);
            }

            return viewModel;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SurveyViewModel : INotifyPropertyChanged
    {
        private string _id;
        private string _title;
        private ObservableCollection<ModuleViewModel> _modules;

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ModuleViewModel> Modules
        {
            get => _modules;
            set
            {
                if (_modules != value)
                {
                    _modules = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ModuleViewModel : INotifyPropertyChanged
    {
        private string _id;
        private string _title;
        private int _rating;
        private ObservableCollection<ThemeViewModel> _themes;

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ModuleRatingDisplay));
                }
            }
        }

        public string ModuleTitle => $"Модуль: {Title}";
        public string ModuleRatingDisplay => $"Оцінка: {Rating}";

        public ObservableCollection<ThemeViewModel> Themes
        {
            get => _themes;
            set
            {
                if (_themes != value)
                {
                    _themes = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ThemeViewModel : INotifyPropertyChanged
    {
        private string _id;
        private string _title;
        private int _rating;
        private ObservableCollection<LessonViewModel> _lessons;

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ThemeRatingDisplay));
                }
            }
        }

        public string ThemeTitle => $"Тема: {Title}";
        public string ThemeRatingDisplay => $"Оцінка: {Rating}";

        public ObservableCollection<LessonViewModel> Lessons
        {
            get => _lessons;
            set
            {
                if (_lessons != value)
                {
                    _lessons = value;
                    OnPropertyChanged();
                }
            }
        }

        public void RecalculateRating()
        {
            Rating = Lessons.Sum(l => l.Rating);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LessonViewModel : INotifyPropertyChanged
    {
        private readonly ModuleViewModel _parentModule;
        private readonly ThemeViewModel _parentTheme;
        private string _id;
        private string _title;
        private int _type;
        private int _rating;
        private string _ratingText;

        public LessonViewModel(ModuleViewModel parentModule, ThemeViewModel parentTheme)
        {
            _parentModule = parentModule;
            _parentTheme = parentTheme;
        }

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LessonTitleWithType));
                }
            }
        }

        public int Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LessonTitleWithType));
                }
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    _ratingText = value > 0 ? value.ToString() : string.Empty;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LessonRatingDisplay));
                    OnPropertyChanged(nameof(LessonRatingText));
                    
                    UpdateParentRatings();
                }
            }
        }

        public string LessonRatingText
        {
            get => _ratingText;
            set
            {
                if (_ratingText != value)
                {
                    _ratingText = value;
                    OnPropertyChanged();
                    
                    if (int.TryParse(value, out int rating))
                    {
                        Rating = rating;
                    }
                }
            }
        }

        public string LessonTitleWithType => $"Завдання: {Title} (Тип: {GetLessonTypeName(Type)})";
        public string LessonRatingDisplay => $"Оцінка: {Rating}";

        private void UpdateParentRatings()
        {
            _parentTheme.RecalculateRating();
            
            _parentModule.Rating = _parentModule.Themes.Sum(t => t.Rating);
        }

        private string GetLessonTypeName(int type)
        {
            return type switch
            {
                0 => "Теорія",
                1 => "Практика",
                2 => "Тест",
                3 => "Завдання",
                4 => "Відео",
                5 => "Аудіо",
                6 => "Презентація",
                _ => $"Тип {type}"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}