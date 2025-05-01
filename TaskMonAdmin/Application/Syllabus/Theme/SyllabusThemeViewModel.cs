using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusThemeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<SyllabusLessonViewModel> _lessons;
        
        public SyllabusThemeViewModel()
        {
            Lessons = new ObservableCollection<SyllabusLessonViewModel>();
        }
        
        public float TotalStudyHours => CalculateTotalStudyHours();
        
        public string TotalStudyHoursDisplay => $"({TotalStudyHours} годин)";
        
        private float CalculateTotalStudyHours()
        {
            float total = 0;
            foreach (var lesson in Lessons)
            {
                if (lesson.StudyHours.HasValue)
                {
                    total += lesson.StudyHours.Value;
                }
            }
            return total;
        }
        
        public static SyllabusThemeViewModel FromModel(Theme theme)
        {
            var viewModel = new SyllabusThemeViewModel
            {
                Title = theme.Title
            };
        
            foreach (var lesson in theme.Lessons)
            {
                viewModel.Lessons.Add(SyllabusLessonViewModel.FromModel(lesson));
            }
        
            return viewModel;
        }
    }
}