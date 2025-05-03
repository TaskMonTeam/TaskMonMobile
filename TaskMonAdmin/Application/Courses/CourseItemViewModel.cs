using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class CourseItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;

        [RelayCommand]
        private async Task SelectCourse()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "courseId", Id.ToString() },
                { "courseTitle", Title }
            };

            await Shell.Current.GoToAsync($"SyllabusGroupPage", navigationParameter);
        }
        
        [RelayCommand]
        private void DeleteCourse()
        {
            DeleteCourseRequested?.Invoke(this, EventArgs.Empty);
        }
        
        [RelayCommand]
        private async Task UpdateCourse()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "courseId", Id.ToString() },
                { "courseTitle", Title }
            };

            await Shell.Current.GoToAsync($"UpdateCoursePage", navigationParameter);
        }

        public event EventHandler? DeleteCourseRequested;

        public static CourseItemViewModel FromModel(Course course)
        {
            return new CourseItemViewModel
            {
                Id = course.Id,
                Title = course.Title
            };
        }
    }
}