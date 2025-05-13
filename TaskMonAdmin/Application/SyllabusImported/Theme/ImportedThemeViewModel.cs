using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ImportService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedThemeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ObservableCollection<ImportedLessonViewModel> _lessons;
        
        public ImportedThemeViewModel()
        {
            Lessons = new ObservableCollection<ImportedLessonViewModel>();
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
        
        public static ImportedThemeViewModel FromModel(Theme theme)
        {
            var viewModel = new ImportedThemeViewModel
            {
                Title = theme.Title
            };
        
            foreach (var lesson in theme.Lessons)
            {
                viewModel.Lessons.Add(ImportedLessonViewModel.FromModel(lesson));
            }
        
            return viewModel;
        }
    }
}