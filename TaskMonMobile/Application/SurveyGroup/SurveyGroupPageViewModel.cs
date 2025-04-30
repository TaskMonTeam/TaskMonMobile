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
        private string _title;

        [ObservableProperty]
        private ObservableCollection<SurveyItemViewModel> _surveys;

        public ICommand NavigateToSurveyCommand { get; }

        public SurveyGroupPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
            Surveys = new ObservableCollection<SurveyItemViewModel>();
            NavigateToSurveyCommand = new Command<Guid>(async (surveyId) => await NavigateToSurvey(surveyId));
        }

        public async Task LoadSurveyGroupDataAsync()
        {
            try
            {
                var groupIdToUse = GroupId != Guid.Empty ? GroupId : Guid.Empty;
                
                var surveyGroup = await _surveyClient.GetSurveyGroupAsync(groupIdToUse);
                LoadFromModel(surveyGroup);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка при завантаженні групи опитувань: {ex.Message}", "OK");
            }
        }

        private void LoadFromModel(SurveyGroup surveyGroup)
        {
            Id = surveyGroup.Id;
            Title = surveyGroup.Title;
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