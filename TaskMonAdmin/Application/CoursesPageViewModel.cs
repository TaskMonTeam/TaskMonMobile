using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Client;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class CoursesPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;
        
        [ObservableProperty]
        private ObservableCollection<CourseItemViewModel> _courses;
        
        [ObservableProperty]
        private bool _isRefreshing;
        
        [ObservableProperty]
        private bool _hasNoCourses;

        public CoursesPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            Courses = new ObservableCollection<CourseItemViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadCoursesAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        public async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await _adminClient.GetCoursesAsync();
                Courses.Clear();
                
                foreach (var course in courses)
                {
                    Courses.Add(CourseItemViewModel.FromModel(course));
                }
                
                HasNoCourses = Courses.Count == 0;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}