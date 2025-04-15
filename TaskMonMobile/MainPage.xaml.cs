using System.Text.Json;
using TaskMonMobile.Common.Models;

namespace TaskMonMobile;

public partial class MainPage : ContentPage
{
    private readonly ISurveyClient _surveyClient;
    private Survey _currentSurvey;
    private Dictionary<string, Entry> _ratingEntries = new Dictionary<string, Entry>();
    private Dictionary<string, Label> _lessonRatingLabels = new Dictionary<string, Label>();
    private Dictionary<string, Label> _themeRatingLabels = new Dictionary<string, Label>();
    private Dictionary<string, Label> _moduleRatingLabels = new Dictionary<string, Label>();

    public MainPage(ISurveyClient surveyClient)
    {
        InitializeComponent();
        _surveyClient = surveyClient;
        LoadSurveyDataAsync();
    }

    private async void LoadSurveyDataAsync()
    {
        try
        {
            _currentSurvey = await _surveyClient.GetSurveyByIdAsync("1e8d6dc3-38ee-4379-9855-08c639962663");
            DisplaySurveyData(_currentSurvey);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
        }
    }

    private void DisplaySurveyData(Survey survey)
    {
        SurveyTitleLabel.Text = survey.Title;
        
        ModulesContainer.Children.Clear();
        _ratingEntries.Clear();
        _lessonRatingLabels.Clear();
        _themeRatingLabels.Clear();
        _moduleRatingLabels.Clear();
        
        foreach (var module in survey.Modules)
        {
            var moduleFrame = new Frame
            {
                Padding = new Thickness(10),
                CornerRadius = 5,
                HasShadow = true,
                BackgroundColor = Colors.LightGray,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var moduleLayout = new VerticalStackLayout
            {
                Spacing = 10
            };

            var moduleHeaderLayout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            moduleHeaderLayout.Children.Add(new Label
            {
                Text = $"Модуль: {module.Title}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            });

            var moduleRatingLabel = new Label
            {
                Text = $"Оцінка: {module.Rating}",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.End
            };
            moduleRatingLabel.SetValue(Grid.ColumnProperty, 1);
            _moduleRatingLabels[module.Id] = moduleRatingLabel;
            
            moduleHeaderLayout.Children.Add(moduleRatingLabel);
            moduleLayout.Children.Add(moduleHeaderLayout);
            
            foreach (var theme in module.Themes)
            {
                var themeFrame = new Frame
                {
                    Padding = new Thickness(10),
                    CornerRadius = 5,
                    HasShadow = true,
                    BackgroundColor = Colors.White,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                var themeLayout = new VerticalStackLayout
                {
                    Spacing = 5
                };

                var themeHeaderLayout = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    }
                };

                themeHeaderLayout.Children.Add(new Label
                {
                    Text = $"Тема: {theme.Title}",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold
                });

                var themeRatingLabel = new Label
                {
                    Text = $"Оцінка: {theme.Rating}",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.End
                };
                themeRatingLabel.SetValue(Grid.ColumnProperty, 1);
                _themeRatingLabels[theme.Id] = themeRatingLabel;
                
                themeHeaderLayout.Children.Add(themeRatingLabel);
                themeLayout.Children.Add(themeHeaderLayout);
                
                foreach (var lesson in theme.Lessons)
                {
                    var lessonLayout = new Grid
                    {
                        Margin = new Thickness(0, 5, 0, 5),
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = new GridLength(120) }
                        },
                        RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto },
                            new RowDefinition { Height = GridLength.Auto }
                        }
                    };
                    
                    lessonLayout.Children.Add(new Label
                    {
                        Text = $"Завдання: {lesson.Title} (Тип: {GetLessonTypeName(lesson.Type)})",
                        FontSize = 16
                    });
                    
                    var ratingLabel = new Label
                    {
                        Text = $"Оцінка: {lesson.Rating}",
                        FontSize = 14,
                        Margin = new Thickness(0, 5, 0, 0)
                    };
                    ratingLabel.SetValue(Grid.RowProperty, 1);
                    _lessonRatingLabels[lesson.Id] = ratingLabel;
                    
                    lessonLayout.Children.Add(ratingLabel);
                    
                    var ratingEntry = new Entry
                    {
                        Placeholder = "Оцініть",
                        Keyboard = Keyboard.Numeric,
                        Text = lesson.Rating > 0 ? lesson.Rating.ToString() : string.Empty,
                        HorizontalOptions = LayoutOptions.End,
                        WidthRequest = 100
                    };
                    ratingEntry.SetValue(Grid.ColumnProperty, 1);
                    
                    ratingEntry.TextChanged += (sender, e) => 
                    {
                        UpdateRating(lesson, theme, module, (Entry)sender);
                    };
                    
                    _ratingEntries[lesson.Id] = ratingEntry;
                    
                    lessonLayout.Children.Add(ratingEntry);

                    themeLayout.Children.Add(lessonLayout);
                }

                themeFrame.Content = themeLayout;
                moduleLayout.Children.Add(themeFrame);
            }

            moduleFrame.Content = moduleLayout;
            ModulesContainer.Children.Add(moduleFrame);
        }
    }

    private void UpdateRating(Lesson lesson, Theme theme, Module module, Entry entry)
    {
        if (int.TryParse(entry.Text, out int rating))
        {
            lesson.Rating = rating;
            
            if (_lessonRatingLabels.TryGetValue(lesson.Id, out var lessonLabel))
            {
                lessonLabel.Text = $"Оцінка: {rating}";
            }
            
            int themeTotal = 0;
            foreach (var themeLesson in theme.Lessons)
            {
                themeTotal += themeLesson.Rating;
            }
            theme.Rating = themeTotal;
            
            if (_themeRatingLabels.TryGetValue(theme.Id, out var themeLabel))
            {
                themeLabel.Text = $"Оцінка: {themeTotal}";
            }
            
            int moduleTotal = 0;
            foreach (var moduleTheme in module.Themes)
            {
                moduleTotal += moduleTheme.Rating;
            }
            module.Rating = moduleTotal;
            
            if (_moduleRatingLabels.TryGetValue(module.Id, out var moduleLabel))
            {
                moduleLabel.Text = $"Оцінка: {moduleTotal}";
            }
        }
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
}