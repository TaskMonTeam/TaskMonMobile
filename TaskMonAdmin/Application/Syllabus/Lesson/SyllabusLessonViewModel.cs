using AdminService.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusLessonViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private LessonType _type;
        
        [ObservableProperty]
        private float? _studyHours;

        public string LessonTypes => $"{Type}";
        public string StudyHoursDisplay => _studyHours.HasValue ? $"{_studyHours} годин" : "не вказано";
        
        public static SyllabusLessonViewModel FromModel(Lesson lesson)
        {
            return new SyllabusLessonViewModel
            {
                Title = lesson.Title,
                Type = lesson.Type,
                StudyHours = lesson.StudyHours
            };
        }
    }
}