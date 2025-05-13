using CommunityToolkit.Mvvm.ComponentModel;
using ImportService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class ImportedLessonViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private LessonType? _type;
        
        [ObservableProperty]
        private float? _studyHours;

        public string LessonTypes => $"{Type}";
        public string StudyHoursDisplay => _studyHours.HasValue ? $"{_studyHours} годин" : "не вказано";
        
        public static ImportedLessonViewModel FromModel(Lesson lesson)
        {
            return new ImportedLessonViewModel
            {
                Title = lesson.Title,
                Type = lesson.Type,
                StudyHours = lesson.StudyHours
            };
        }
    }
}