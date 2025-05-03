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
        private Guid _courseId;

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
            var navigationParameter = new Dictionary<string, object>
            {
                { "courseId", CourseId.ToString() }
            };
            await Shell.Current.GoToAsync("CreateSyllabusPage", navigationParameter);
        }
        
        public async Task LoadSyllabusesAsync()
        {
            try
            {
                var syllabuses = await _adminClient.GetCourseSyllabusesAsync(CourseId);
                Syllabuses.Clear();
                
                foreach (var syllabus in syllabuses)
                {
                    var viewModel = SyllabusGroupItemViewModel.FromModel(syllabus);
                    viewModel.CourseId = CourseId;
                    Syllabuses.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}