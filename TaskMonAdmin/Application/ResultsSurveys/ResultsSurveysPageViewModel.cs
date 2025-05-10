using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatisticsService.Client;

namespace TaskMonAdmin.ViewModels
{
    public partial class ResultsSurveysPageViewModel : ObservableObject
    {
        private readonly IStatisticsClient _statisticsClient;
        
        [ObservableProperty]
        private ObservableCollection<SurveyResultItemViewModel> _surveyResults;
        
        [ObservableProperty]
        private bool _isRefreshing;
        
        [ObservableProperty]
        private bool _hasError;
        
        [ObservableProperty]
        private string _errorMessage;

        public ResultsSurveysPageViewModel(IStatisticsClient statisticsClient)
        {
            _statisticsClient = statisticsClient;
            SurveyResults = new ObservableCollection<SurveyResultItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveyResultsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task NavigateToTimeline(Guid surveyId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "surveyId", surveyId }
            };
            
            await Shell.Current.GoToAsync("TimelinePage", navigationParameter);
        }
        
        public async Task LoadSurveyResultsAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;
            
            try
            {
                var results = await _statisticsClient.GetSurveyResults();
                SurveyResults.Clear();
                
                foreach (var result in results)
                {
                    var viewModel = SurveyResultItemViewModel.FromModel(result);
                    SurveyResults.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Виникла помилка: {ex.Message}";
                await Application.Current.MainPage.DisplayAlert("Помилка", ErrorMessage, "OK");
            }
        }
    }
}