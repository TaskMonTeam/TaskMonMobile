using SurveyService.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using SurveyService.Models;

namespace TaskMonMobile.ViewModels
{
    public partial class SurveyPageViewModel : ObservableObject
    {
        private readonly ISurveyClient _surveyClient;
    
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<ModuleViewModel> _modules;

        public SurveyPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
            Modules = new ObservableCollection<ModuleViewModel>();
        }

        public async Task LoadSurveyDataAsync()
        {
            try
            {
                var survey = await _surveyClient.GetSurveyAsync(Guid.Empty);
                LoadFromModel(survey);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }

        private void LoadFromModel(Survey survey)
        {
            Id = survey.Id;
            Title = survey.Title;
            Modules.Clear();
            
            foreach (var module in survey.Modules)
            {
                Modules.Add(ModuleViewModel.FromModel(module));
            }
        }
    }
}