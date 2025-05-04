using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Client;
using AdminService.Client.Requests;

namespace TaskMonAdmin.ViewModels
{
    public partial class CreateSurveyGroupPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private ObservableCollection<SurveyPickerItemViewModel> _surveyPickers;

        [ObservableProperty]
        private ObservableCollection<SurveyItemViewModel> _availableSurveys;

        public CreateSurveyGroupPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            SurveyPickers = new ObservableCollection<SurveyPickerItemViewModel> 
            { 
                new() 
            };
            AvailableSurveys = new ObservableCollection<SurveyItemViewModel>();
        }

        public async Task LoadSurveysAsync()
        {
            try
            {
                var surveys = await _adminClient.GetSurveysAsync();
                AvailableSurveys.Clear();
                
                foreach (var survey in surveys)
                {
                    var viewModel = SurveyItemViewModel.FromModel(survey);
                    AvailableSurveys.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка при завантаженні опитувань: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private void AddSurveyPicker()
        {
            SurveyPickers.Add(new());
        }

        [RelayCommand]
        private void RemoveSurveyPicker(SurveyPickerItemViewModel pickerViewModel)
        {
            if (SurveyPickers.Count > 1)
            {
                SurveyPickers.Remove(pickerViewModel);
            }
        }

        [RelayCommand]
        private async Task CreateSurveyGroup()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Назва групи оінювань не може бути порожньою", "OK");
                return;
            }

            var selectedSurveyIds = SurveyPickers
                .Where(p => p.SelectedSurvey is not null)
                .Select(p => p.SelectedSurvey!.Id)
                .ToList();

            if (!selectedSurveyIds.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Додайте хоча б одне опитування", "OK");
                return;
            }
            
            try
            {
                var request = new CreateSurveyGroupRequest(
                    Title,
                    Description,
                    selectedSurveyIds);

                await _adminClient.CreateSurveyGroupAsync(request);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}