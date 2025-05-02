using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Client;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusGroupPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;
        
        [ObservableProperty]
        private ObservableCollection<SyllabusGroupItemViewModel> _syllabuses;
        
        [ObservableProperty]
        private bool _isRefreshing;
        
        [ObservableProperty]
        private bool _hasNoSyllabuses;

        public SyllabusGroupPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            Syllabuses = new ObservableCollection<SyllabusGroupItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSyllabusesAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        [RelayCommand]
        private async Task CreateSyllabus()
        {
            await Shell.Current.GoToAsync("CreateSyllabusPage");
        }
        
        public async Task LoadSyllabusesAsync()
        {
            try
            {
                var syllabuses = await _adminClient.GetCourseSyllabusesAsync(Guid.Empty);
                Syllabuses.Clear();
                
                foreach (var syllabus in syllabuses)
                {
                    Syllabuses.Add(SyllabusGroupItemViewModel.FromModel(syllabus));
                }
                
                HasNoSyllabuses = Syllabuses.Count == 0;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}