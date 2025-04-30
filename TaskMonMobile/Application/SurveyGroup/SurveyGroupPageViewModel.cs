using SurveyService.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
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

        public ICommand NavigateToSurveyCommand { get; }

        public SurveyGroupPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
            Surveys = new ObservableCollection<SurveyItemViewModel>();
            NavigateToSurveyCommand = new Command<Guid>(async (surveyId) => await NavigateToSurvey(surveyId));
            HasNoSurveys = true;
        }

        public async Task LoadSurveyGroupDataAsync()
        {
            try
            {
                IsLoading = true;
                var groupIdToUse = GroupId != Guid.Empty ? GroupId : Guid.Empty;
                
                if (groupIdToUse == Guid.Empty)
                {
                    HasNoSurveys = true;
                    Title = "Опитування";
                    IsLoading = false;
                    return;
                }
                
                var surveyGroup = await _surveyClient.GetSurveyGroupAsync(groupIdToUse);
                LoadFromModel(surveyGroup);
                
                HasNoSurveys = Surveys.Count == 0;
                
                if (HasNoSurveys || string.IsNullOrEmpty(Title))
                {
                    Title = "Опитування";
                }
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

        private async Task NavigateToSurvey(Guid surveyId)
        {
            await Shell.Current.GoToAsync($"SurveyPage?surveyId={surveyId}");
        }
    }
}