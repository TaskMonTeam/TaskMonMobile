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

namespace TaskMonAdmin.ViewModels;

public partial class MonitoringPageViewModel : ObservableObject
{
    private readonly IStatisticsClient _statisticsClient;
    private SurveyGroupResultsTimeline _surveyResults;

    [ObservableProperty]
    private ObservableCollection<SurveyCheckBoxItem> _surveyCheckBoxes = [];

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

    public MonitoringPageViewModel(IStatisticsClient statisticsClient)
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
            _surveyResults = await _statisticsClient.GetSurveyGroupResultsTimeline(Guid.Empty);
            
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
            
            UpdateChart();
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

        var strokeDashArray = new float[] { 3 , 2, 3, 2, 1, 2};
        var effect = new DashEffect(strokeDashArray);
        
        foreach (var surveyStatistic in surveyStatistics)
        {
            var surveyName = surveyStatistic.Name.Substring(0,10);
            var actualPoints = surveyStatistic.Statistics.DataPoints;
            var predictedPoints = surveyStatistic.Statistics.PredictedPoints;

            lineSeries.Add(
                new LineSeries<float>
                {
                    Values = actualPoints,
                    Name = $"{surveyName}: Факт",
                    Stroke = new SolidColorPaint()
                    {
                        Color = SKColors.Green,
                        StrokeThickness = 2
                    },
                    GeometrySize = 8
                });
            
            lineSeries.Add(
                new LineSeries<float> 
                { 
                    Values = predictedPoints, 
                    Name = $"{surveyName}: Прогноз",
                    Stroke = new SolidColorPaint()
                    {
                        Color = SKColors.Green,
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