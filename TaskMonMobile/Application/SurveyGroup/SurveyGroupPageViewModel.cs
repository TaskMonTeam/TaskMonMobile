using SurveyService.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SurveyService.Models;

namespace TaskMonMobile.ViewModels
{
    public partial class SurveyGroupPageViewModel : ObservableObject
    {
        private readonly ISurveyClient _surveyClient;
    
        [ObservableProperty]
        private Guid _id;
        
        [ObservableProperty]
        private Guid _groupId;

        [ObservableProperty]
        private string _title = "Опитування";

        [ObservableProperty]
        private ObservableCollection<SurveyItemViewModel> _surveys;
        
        [ObservableProperty]
        private bool _hasNoSurveys;
        
        [ObservableProperty]
        private string _noSurveysMessage = "У вас поки немає опитувань";

        [ObservableProperty]
        private bool _isLoading;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public ICommand NavigateToSurveyCommand { get; }

        public SurveyGroupPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
            Surveys = new ObservableCollection<SurveyItemViewModel>();
            NavigateToSurveyCommand = new Command<Guid>(async (surveyId) => await NavigateToSurvey(surveyId));
            HasNoSurveys = true;
        }
        
        [RelayCommand]
        private async Task RefreshDataAsync()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveyGroupDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task LoadSurveyGroupDataAsync()
        {
            try
            {
                IsLoading = true;
                var surveyGroup = await _surveyClient.GetSurveyGroupAsync(GroupId);
                LoadFromModel(surveyGroup);
                
                HasNoSurveys = Surveys.Count == 0;
                
                if (HasNoSurveys || string.IsNullOrEmpty(Title))
                {
                    Title = "Опитування";
                }
                
                LoadCompletedSurveyStatus();
            }
            catch (Exception ex)
            {
                HasNoSurveys = true;
                NoSurveysMessage = "Не вірне посилання, спробуйте знову";
                Title = "Опитування";
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadFromModel(SurveyGroup surveyGroup)
        {
            Id = surveyGroup.Id;
            
            Title = !string.IsNullOrEmpty(surveyGroup.Title) ? surveyGroup.Title : "Опитування";
            
            Surveys.Clear();
            
            foreach (var survey in surveyGroup.Surveys)
            {
                Surveys.Add(new SurveyItemViewModel
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    ParentViewModel = this
                });
            }
        }
        
        private void LoadCompletedSurveyStatus()
        {
            foreach (var survey in Surveys)
            {
                string key = $"CompletedSurvey_{GroupId}_{survey.Id}";
                bool isCompleted = Preferences.Get(key, false);
                survey.IsCompleted = isCompleted;
            }
        }
        
        public void MarkSurveyAsCompleted(Guid surveyId)
        {
            var survey = Surveys.FirstOrDefault(s => s.Id == surveyId);
            if (survey != null)
            {
                survey.IsCompleted = true;
                string key = $"CompletedSurvey_{GroupId}_{surveyId}";
                Preferences.Set(key, true);
            }
        }

        private async Task NavigateToSurvey(Guid surveyId)
        {
            await Shell.Current.GoToAsync($"SurveyPage?surveyId={surveyId}&groupId={GroupId}");
        }
    }
}