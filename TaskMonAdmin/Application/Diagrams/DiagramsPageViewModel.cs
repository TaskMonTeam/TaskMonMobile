using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatisticsService.Client;
using StatisticsService.Client.Models;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using StatisticsService.Client.Models.StudentCategorization;

namespace TaskMonAdmin.ViewModels;

public partial class DiagramsPageViewModel : ObservableObject
{
    private readonly IStatisticsClient _statisticsClient;

    [ObservableProperty]
    private ISeries[] _series;
    
    [ObservableProperty]
    private IEnumerable<ISeries> _pieSeries;

    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private Guid _surveyId;
    
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

    public DiagramsPageViewModel(IStatisticsClient statisticsClient)
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
            
            var timelineResults = await _statisticsClient.GetSurveyResultsTimeline(SurveyId);
            UpdateChart(timelineResults.SurveyTimeline);
            
            var categorizationResults = await _statisticsClient.GetSurveyResultsCategorization(SurveyId);
            UpdatePieChart(categorizationResults.Categorization.Distribution);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", 
                $"Не вдалося завантажити дані опитування: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void UpdateChart(Timeline timeline)
    {
        var actualColor = SKColors.DodgerBlue;
        var predictedColor = SKColors.DodgerBlue;
        var strokeDashArray = new float[] { 3, 2, 3, 2 };
        var dashEffect = new DashEffect(strokeDashArray);
        
        List<LineSeries<float>> lineSeries = [];

        lineSeries.Add(new LineSeries<float>
        {
            Values = timeline.DataPoints,
            Name = "Фактичні дані",
            Stroke = new SolidColorPaint()
            {
                Color = actualColor,
                StrokeThickness = 2
            },
            Fill = new SolidColorPaint(actualColor.WithAlpha(80)),
            GeometrySize = 8,
            GeometryStroke = new SolidColorPaint(actualColor, 2),
            GeometryFill = new SolidColorPaint(SKColors.White)
        });
        lineSeries.Add(new LineSeries<float>
        {
            Values = timeline.PredictedPoints,
            Name = "Прогнозовані дані",
            Stroke = new SolidColorPaint()
            {
                Color = predictedColor,
                StrokeThickness = 1,
                PathEffect = dashEffect,
            },
            Fill = new SolidColorPaint(SKColors.Transparent),
            GeometrySize = 0
        });
    
        Series = lineSeries.ToArray();
        OnPropertyChanged(nameof(Series));
    }
    
    private void UpdatePieChart(IDictionary<WorkloadCategories, int> distribution)
    {
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
}