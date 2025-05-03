using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Client;
using AdminService.Client.Requests;

namespace TaskMonAdmin.ViewModels
{
    public partial class CreateCoursePageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;

        [ObservableProperty]
        private string _courseTitle;
        
        public CreateCoursePageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
        }

        [RelayCommand]
        private async Task CreateCourse()
        {
            if (string.IsNullOrWhiteSpace(CourseTitle))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Назва курсу не може бути порожньою", "OK");
                return;
            }

            try
            {
                var request = new CreateCourseRequest(CourseTitle);

                await _adminClient.CreateCourseAsync(request);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}