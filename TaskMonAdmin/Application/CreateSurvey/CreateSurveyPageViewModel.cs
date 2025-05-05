using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Client;
using AdminService.Client.Requests;

namespace TaskMonAdmin.ViewModels
{
    public partial class CreateSurveyPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private CourseItemViewModel? _selectedCourse;

        [ObservableProperty]
        private ObservableCollection<CourseItemViewModel> _availableCourses;

        public CreateSurveyPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            AvailableCourses = new ObservableCollection<CourseItemViewModel>();
        }

        public async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await _adminClient.GetCoursesAsync();
                AvailableCourses.Clear();
                
                foreach (var course in courses)
                {
                    var viewModel = CourseItemViewModel.FromModel(course);
                    AvailableCourses.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка при завантаженні курсів: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task CreateSurvey()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Назва опитування не може бути порожньою", "OK");
                return;
            }

            if (SelectedCourse == null)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", "Будь ласка, виберіть курс", "OK");
                return;
            }
            
            try
            {
                var request = new CreateSurveyForCourseRequest(
                    Title,
                    Description);

                await _adminClient.CreateSurveyForCourseAsync(SelectedCourse.Id, request);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }
    }
}