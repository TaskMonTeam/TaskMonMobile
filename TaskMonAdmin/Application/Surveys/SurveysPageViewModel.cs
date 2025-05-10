using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Client;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveysPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;
        
        [ObservableProperty]
        private ObservableCollection<SurveyItemViewModel> _surveys;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public SurveysPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            Surveys = new ObservableCollection<SurveyItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveysAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task CreateSurvey()
        {
            await Shell.Current.GoToAsync("CreateSurveyPage");
        }

        [RelayCommand]
        private async Task OpenSurveyLink(SurveyItemViewModel survey)
        {
            var link = $"https://taskmon.com/surveys/invite/{survey.Id}";
            var navigationParameter = new Dictionary<string, object>
            {
                { "Link", link }
            };
            
            await Shell.Current.GoToAsync("LinkPage", navigationParameter);
        }
        
        public async Task LoadSurveysAsync()
        {
            try
            {
                var surveys = await _adminClient.GetSurveysAsync();
                Surveys.Clear();
                
                foreach (var survey in surveys)
                {
                    var viewModel = SurveyItemViewModel.FromModel(survey);
                    Surveys.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}