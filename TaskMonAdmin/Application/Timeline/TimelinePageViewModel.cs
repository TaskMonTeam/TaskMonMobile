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

namespace TaskMonAdmin.ViewModels;

public partial class TimelinePageViewModel : ObservableObject
{
    private readonly IStatisticsClient _statisticsClient;

    [ObservableProperty]
    private ISeries[] _series;

    [ObservableProperty]
    private bool _isLoading;

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

    public TimelinePageViewModel(IStatisticsClient statisticsClient)
    {
        _statisticsClient = statisticsClient;
        Series = [];

        _ = LoadSurveyData();
    }

    [RelayCommand]
    private async Task LoadSurveyData()
    {
        try
        {
            IsLoading = true;
            var surveyResults = await _statisticsClient.GetSurveyResultsTimeline(Guid.Empty);
            
            UpdateChart(surveyResults.SurveyTimeline);
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
}