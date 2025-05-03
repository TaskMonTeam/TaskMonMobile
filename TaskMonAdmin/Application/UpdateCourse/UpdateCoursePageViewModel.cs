using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Client;
using AdminService.Client.Requests;

namespace TaskMonAdmin.ViewModels
{
    public partial class UpdateCoursePageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;

        [ObservableProperty]
        private string _courseTitle;

        [ObservableProperty]
        private Guid _courseId;
        
        public UpdateCoursePageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        public void Initialize(Guid courseId, string courseTitle)
        {
            CourseId = courseId;
            CourseTitle = courseTitle;
        }

        [RelayCommand]
        private async Task UpdateCourse()
        {
            if (string.IsNullOrWhiteSpace(CourseTitle))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Назва курсу не може бути порожньою", "OK");
                return;
            }

            try
            {
                var request = new UpdateCourseTitleRequest(CourseTitle);

                await _adminClient.UpdateCourse(CourseId, request);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}