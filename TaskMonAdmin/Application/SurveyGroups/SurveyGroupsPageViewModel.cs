using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Client;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SurveyGroupsPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;
        
        [ObservableProperty]
        private ObservableCollection<SurveyGroupItemViewModel> _surveyGroups;
        
        [ObservableProperty]
        private bool _isRefreshing;

        public SurveyGroupsPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            SurveyGroups = new ObservableCollection<SurveyGroupItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSurveyGroupsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task CreateSurveyGroupPage()
        {
            await Shell.Current.GoToAsync("CreateSurveyGroupPage");
        }

        [RelayCommand]
        private async Task OpenSurveyGroupLink(SurveyGroupItemViewModel surveyGroup)
        {
            var link = $"https://taskmon.com/groups/invite/{surveyGroup.Id}";
            var navigationParameter = new Dictionary<string, object>
            {
                { "Link", link }
            };
            
            await Shell.Current.GoToAsync("LinkPage", navigationParameter);
        }
        
        public async Task LoadSurveyGroupsAsync()
        {
            try
            {
                var surveyGroups = await _adminClient.GetSurveyGroupsAsync();
                SurveyGroups.Clear();
                
                foreach (var group in surveyGroups)
                {
                    var viewModel = SurveyGroupItemViewModel.FromModel(group);
                    SurveyGroups.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}