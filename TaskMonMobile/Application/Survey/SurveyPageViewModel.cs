using SurveyService.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using SurveyService.Models;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonMobile.ViewModels
{
    public partial class SurveyPageViewModel : ObservableObject
    {
        private readonly ISurveyClient _surveyClient;
    
        [ObservableProperty]
        private Guid _id;
        
        [ObservableProperty]
        private Guid? _groupId;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<ModuleViewModel> _modules;
        
        [ObservableProperty]
        private bool _isSubmitting;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public SurveyPageViewModel(ISurveyClient surveyClient)
        {
            _surveyClient = surveyClient;
            Modules = new ObservableCollection<ModuleViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshDataAsync()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveyDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        public async Task LoadSurveyDataAsync()
        {
            try
            {
                var survey = await _surveyClient.GetSurveyAsync(Id);
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
        
        [RelayCommand]
        private async Task SubmitSurvey()
        {
            if (IsSubmitting)
                return;

            try
            {
                IsSubmitting = true;
                var assessments = new List<Assessment>();
            
                foreach (var module in Modules)
                {
                    foreach (var theme in module.Themes)
                    {
                        foreach (var lesson in theme.Lessons)
                        {
                            if (lesson.TimeSpend > 0)
                            {
                                var assessment = new Assessment(
                                    lesson.Id,
                                    lesson.TimeSpend
                                );
                                assessments.Add(assessment);
                            }
                        }
                    }
                }
            
                if (assessments.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Не відправлено", "Вкажіть час хоча б для одного завдання.", "OK");
                    return;
                }
                
                Submission submission = GroupId.HasValue
                    ? new Submission(assessments, GroupId.Value)
                    : new Submission(assessments);
                
                await _surveyClient.SubmitSurveyAsync(Id, submission);
                
                if (GroupId.HasValue)
                {
                    string key = $"CompletedSurvey_{GroupId.Value}_{Id}";
                    Preferences.Set(key, true);
                }
                
                await Application.Current.MainPage.DisplayAlert("Відправлено", "Дякуємо за відповідь!", "OK");
                
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Не вдалося відправити записи часу: {ex.Message}", "OK");
            }
            finally
            {
                IsSubmitting = false;
            }
        }
    }
}