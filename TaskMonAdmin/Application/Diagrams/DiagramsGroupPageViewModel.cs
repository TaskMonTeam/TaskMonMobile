using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatisticsService.Client;
using StatisticsService.Client.Models;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using StatisticsService.Client.Models.StudentCategorization;

namespace TaskMonAdmin.ViewModels;

public partial class DiagramsGroupPageViewModel : ObservableObject
{
    private readonly IStatisticsClient _statisticsClient;
    private SurveyGroupResultsTimeline _surveyResults;
    private SurveyGroupResultsCategorization _categorizationResults;

    [ObservableProperty]
    private ObservableCollection<SurveyCheckBoxItem> _surveyCheckBoxes = [];

    [ObservableProperty]
    private ObservableCollection<DistributionPickerItem> _distributionItems = [];

    [ObservableProperty]
    private DistributionPickerItem _selectedDistributionItem;

    [ObservableProperty]
    private ISeries[] _series;

    [ObservableProperty]
    private IEnumerable<ISeries> _pieSeries;

    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private Guid _groupId;

    private readonly Dictionary<WorkloadCategories, SKColor> _categoryColors = new()
    {
        { WorkloadCategories.OnTrack, SKColors.ForestGreen },
        { WorkloadCategories.Overload20, SKColors.Orange },
        { WorkloadCategories.Overload50, SKColors.OrangeRed },
        { WorkloadCategories.CriticalOverload, SKColors.Crimson },
        { WorkloadCategories.Underload20, SKColors.SkyBlue },
        { WorkloadCategories.Underload35, SKColors.RoyalBlue },
        { WorkloadCategories.CriticalUnderload, SKColors.DarkBlue }
    };

    private readonly Dictionary<WorkloadCategories, string> _categoryNames = new()
    {
        { WorkloadCategories.OnTrack, "В нормі" },
        { WorkloadCategories.Overload20, "Перевантажені на 20%" },
        { WorkloadCategories.Overload50, "Перевантажені на 50%" },
        { WorkloadCategories.CriticalOverload, "Критично перевантажені" },
        { WorkloadCategories.Underload20, "Недовантажені на 20%" },
        { WorkloadCategories.Underload35, "Недовантажені на 35%" },
        { WorkloadCategories.CriticalUnderload, "Критично недовантажені" }
    };

    public ICartesianAxis[] XAxes { get; set; } = [
        new Axis
        {
            CrosshairLabelsBackground = SKColors.DarkOrange.AsLvcColor(),
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            Labeler = value => value.ToString("N0")
        }
    ];
 
    public ICartesianAxis[] YAxes { get; set; } = [
        new Axis
        {
            CrosshairLabelsBackground = SKColors.DarkOrange.AsLvcColor(),
            CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed),
            CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
            Labeler = value => value.ToString("N1"),
            CrosshairSnapEnabled = true
        }
    ];

    public DiagramsGroupPageViewModel(IStatisticsClient statisticsClient)
    {
        _statisticsClient = statisticsClient;
        Series = [];
        PieSeries = [];
    }

    [RelayCommand]
    public async Task LoadSurveyData()
    {
        try
        {
            IsLoading = true;
            
            _surveyResults = await _statisticsClient.GetSurveyGroupResultsTimeline(GroupId);
            
            SurveyCheckBoxes.Clear();
            
            foreach (var surveyItem in _surveyResults.Statistics)
            {
                SurveyCheckBoxes.Add(new SurveyCheckBoxItem
                {
                    Name = TruncateName(surveyItem.Survey.SurveyName, 20),
                    IsSelected = false,
                    Statistics = surveyItem.Timeline,
                });
            }
            
            SurveyCheckBoxes.Add(new SurveyCheckBoxItem
            {
                Name = TruncateName("Загальні дані", 20),
                IsSelected = false,
                Statistics = _surveyResults.GlobalTimeline,
            });

            SurveyCheckBoxes.Last().IsSelected = true;
            
            _categorizationResults = await _statisticsClient.GetSurveyGroupResultsCategorization(GroupId);
            
            DistributionItems.Clear();
            
            DistributionItems.Add(new DistributionPickerItem
            {
                Name = "Загальні дані",
                Distribution = _categorizationResults.Global.Distribution,
                IsSelected = true
            });
            
            foreach (var survey in _categorizationResults.Surveys)
            {
                DistributionItems.Add(new DistributionPickerItem
                {
                    Name = TruncateName(survey.Survey.SurveyName, 20),
                    Distribution = survey.Categorization.Distribution,
                    IsSelected = false
                });
            }
            
            SelectedDistributionItem = DistributionItems.First();
            
            UpdateChart();
            UpdatePieChart();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", 
                $"Не вдалося завантажити дані опитувань: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private string TruncateName(string name, int maxLength)
    {
        if (string.IsNullOrEmpty(name) || name.Length <= maxLength)
            return name;
            
        return name.Substring(0, maxLength - 3) + "...";
    }
    
    [RelayCommand]
    private void UpdateChart()
    {
        IEnumerable<SurveyCheckBoxItem> surveyStatistics = SurveyCheckBoxes.Where(cb => cb.IsSelected);
        List<LineSeries<float>> lineSeries = [];

        var strokeDashArray = new float[] { 3, 2, 3, 2, 1, 2 };
        var effect = new DashEffect(strokeDashArray);
    
        var colors = new[] 
        {
            SKColors.DodgerBlue,
            SKColors.Crimson,
            SKColors.ForestGreen,
            SKColors.Orange,
            SKColors.Purple,
            SKColors.Teal,
            SKColors.Brown,
            SKColors.DarkGoldenrod
        };
    
        int colorIndex = 0;
    
        foreach (var surveyStatistic in surveyStatistics)
        {
            var surveyName = surveyStatistic.Name.Substring(0, Math.Min(10, surveyStatistic.Name.Length));
            var actualPoints = surveyStatistic.Statistics.DataPoints;
            var predictedPoints = surveyStatistic.Statistics.PredictedPoints;
        
            var currentColor = colors[colorIndex % colors.Length];
            colorIndex++;
        
            lineSeries.Add(
                new LineSeries<float>
                {
                    Values = actualPoints,
                    Name = $"{surveyName}: Факт",
                    Stroke = new SolidColorPaint()
                    {
                        Color = currentColor,
                        StrokeThickness = 2
                    },
                    Fill = new SolidColorPaint(currentColor.WithAlpha(80)),
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(currentColor, 2),
                    GeometryFill = new SolidColorPaint(SKColors.White)
                });
        
            lineSeries.Add(
                new LineSeries<float> 
                { 
                    Values = predictedPoints, 
                    Name = $"{surveyName}: Прогноз",
                    Stroke = new SolidColorPaint()
                    {
                        Color = currentColor,
                        StrokeThickness = 1,
                        PathEffect = effect,
                    },
                    Fill = new SolidColorPaint(SKColors.Transparent),
                    GeometrySize = 0
                });
        }
    
        Series = lineSeries.ToArray();
        OnPropertyChanged(nameof(Series));
    }
    
    [RelayCommand]
    private void UpdatePieChart()
    {
        var distribution = SelectedDistributionItem.Distribution;
        
        if (distribution.Count == 0)
        {
            PieSeries = [];
            OnPropertyChanged(nameof(PieSeries));
            return;
        }
        
        var values = new List<ISeries>();
        
        foreach (var item in distribution)
        {
            if (item.Value > 0)
            {
                var category = item.Key;
                var count = item.Value;
                var categoryName = _categoryNames.TryGetValue(category, out var name) ? name : category.ToString();
                var categoryColor = _categoryColors.TryGetValue(category, out var color) ? color : SKColors.Gray;
                
                values.Add(new PieSeries<int>
                {
                    Values = new[] { count },
                    Name = categoryName,
                    Fill = new SolidColorPaint(categoryColor)
                });
            }
        }
        
        PieSeries = values;
        OnPropertyChanged(nameof(PieSeries));
    }
    
    partial void OnSelectedDistributionItemChanged(DistributionPickerItem value)
    {
        if (value != null)
        {
            UpdatePieChart();
        }
    }
}

public partial class SurveyCheckBoxItem : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private Timeline _statistics;
}

public partial class DistributionPickerItem : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private IDictionary<WorkloadCategories, int> _distribution;

    [ObservableProperty]
    private bool _isSelected;
    
    public override string ToString()
    {
        return Name;
    }
}