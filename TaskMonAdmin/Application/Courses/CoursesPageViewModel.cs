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

        [RelayCommand]
        private async Task CreateCourse()
        {
            await Shell.Current.GoToAsync("CreateCoursePage");
        }
        
        public async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await _adminClient.GetCoursesAsync();
                Courses.Clear();
                
                foreach (var course in courses)
                {
                    var courseViewModel = CourseItemViewModel.FromModel(course);
                    courseViewModel.DeleteCourseRequested += OnDeleteCourseRequested;
                    Courses.Add(courseViewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteCourseRequested(object? sender, EventArgs e)
        {
            if (sender is CourseItemViewModel courseViewModel)
            {
                try
                {
                    await _adminClient.DeleteCourseAsync(courseViewModel.Id);
                    Courses.Remove(courseViewModel);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Помилка", $"Не вдалося видалити курс: {ex.Message}", "OK");
                }
            }
        }
    }
}