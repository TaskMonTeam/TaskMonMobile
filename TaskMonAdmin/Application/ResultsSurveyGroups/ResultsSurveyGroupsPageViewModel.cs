using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatisticsService.Client;

namespace TaskMonAdmin.ViewModels
{
    public partial class ResultsSurveyGroupsPageViewModel : ObservableObject
    {
        private readonly IStatisticsClient _statisticsClient;
        
        [ObservableProperty]
        private ObservableCollection<SurveyGroupResultItemViewModel> _surveyGroupResults;
        
        [ObservableProperty]
        private bool _isRefreshing;
        
        [ObservableProperty]
        private bool _hasError;
        
        [ObservableProperty]
        private string _errorMessage;

        public ResultsSurveyGroupsPageViewModel(IStatisticsClient statisticsClient)
        {
            _statisticsClient = statisticsClient;
            SurveyGroupResults = new ObservableCollection<SurveyGroupResultItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveyGroupResultsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task NavigateToTimeline(Guid groupId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "groupId", groupId }
            };
            
            await Shell.Current.GoToAsync("DiagramsGroupPage", navigationParameter);
        }
        
        public async Task LoadSurveyGroupResultsAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;
            
            try
            {
                var results = await _statisticsClient.GetSurveyGroupResults();
                SurveyGroupResults.Clear();
                
                foreach (var result in results)
                {
                    var viewModel = SurveyGroupResultItemViewModel.FromModel(result);
                    SurveyGroupResults.Add(viewModel);
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