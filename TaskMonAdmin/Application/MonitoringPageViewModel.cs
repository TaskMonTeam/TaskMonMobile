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

namespace TaskMonAdmin.ViewModels;

public partial class MonitoringPageViewModel : ObservableObject
{
    private readonly IStatisticsClient _statisticsClient;
    private SurveyGroupResults _surveyResults;

    [ObservableProperty]
    private ObservableCollection<SurveyCheckBoxItem> _surveyCheckBoxes = new();

    [ObservableProperty]
    private ISeries[] _series;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _selectedSurveyName = "Усі";

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
            CrosshairSnapEnabled = true
        }
    ];

    public MonitoringPageViewModel(IStatisticsClient statisticsClient)
    {
        _statisticsClient = statisticsClient;
        Series = [
            new LineSeries<double> 
            { 
                Values = [], 
                Name = "Фактичні дані",
                Stroke = new SolidColorPaint(SKColors.Blue, 2),
                GeometryStroke = new SolidColorPaint(SKColors.Blue, 2)
            },
            new LineSeries<double> 
            { 
                Values = [], 
                Name = "Прогнозовані дані",
                Stroke = new SolidColorPaint(SKColors.Green, 2),
                GeometryStroke = new SolidColorPaint(SKColors.Green, 2),
                Fill = new SolidColorPaint(SKColors.Green.WithAlpha(40))
            }
        ];

        _ = LoadSurveyData();
    }

    [RelayCommand]
    private async Task LoadSurveyData()
    {
        try
        {
            IsLoading = true;
            _surveyResults = await _statisticsClient.GetSurveyGroupResults(Guid.Empty);
            
            SurveyCheckBoxes.Clear();
            SurveyCheckBoxes.Add(new SurveyCheckBoxItem 
            { 
                Name = "Усі", 
                IsSelected = true,
                SurveyId = Guid.Empty
            });
            
            foreach (var surveyItem in _surveyResults.Statistics)
            {
                SurveyCheckBoxes.Add(new SurveyCheckBoxItem
                {
                    Name = surveyItem.Survey.SurveyName,
                    IsSelected = false,
                    SurveyId = surveyItem.Survey.SurveyId
                });
            }
            
            UpdateChartWithGlobalStatistics();
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

    [RelayCommand]
    private void SelectSurvey(SurveyCheckBoxItem selectedItem)
    {
        foreach (var item in SurveyCheckBoxes)
        {
            item.IsSelected = false;
        }
        
        selectedItem.IsSelected = true;
        SelectedSurveyName = selectedItem.Name;
        
        if (selectedItem.Name == "Усі")
        {
            UpdateChartWithGlobalStatistics();
        }
        else
        {
            UpdateChartWithSurveyStatistics(selectedItem.SurveyId);
        }
    }

    private void UpdateChartWithGlobalStatistics()
    {
        if (_surveyResults?.GlobalStatistics == null)
            return;

        var actualPoints = _surveyResults.GlobalStatistics.DataPoints
            .Select(p => (double)p)
            .ToArray();

        var predictedPoints = _surveyResults.GlobalStatistics.PredictedPoints
            .Select(p => (double)p)
            .ToArray();

        UpdateChart(actualPoints, predictedPoints);
    }

    private void UpdateChartWithSurveyStatistics(Guid surveyId)
    {
        var surveyStats = _surveyResults?.Statistics?.FirstOrDefault(s => s.Survey.SurveyId == surveyId);
        if (surveyStats?.Statistics == null)
            return;

        var actualPoints = surveyStats.Statistics.DataPoints
            .Select(p => (double)p)
            .ToArray();

        var predictedPoints = surveyStats.Statistics.PredictedPoints
            .Select(p => (double)p)
            .ToArray();

        UpdateChart(actualPoints, predictedPoints);
    }

    private void UpdateChart(double[] actualPoints, double[] predictedPoints)
    {
        Series = [
            new LineSeries<double> 
            { 
                Values = actualPoints, 
                Name = "Фактичні дані",
                Stroke = new SolidColorPaint(SKColors.Blue, 2),
                GeometryStroke = new SolidColorPaint(SKColors.Blue, 2)
            },
            new LineSeries<double> 
            { 
                Values = predictedPoints, 
                Name = "Прогнозовані дані",
                Stroke = new SolidColorPaint(SKColors.Green, 2),
                GeometryStroke = new SolidColorPaint(SKColors.Green, 2),
                Fill = new SolidColorPaint(SKColors.Green.WithAlpha(40))
            }
        ];

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
    private Guid _surveyId;
}