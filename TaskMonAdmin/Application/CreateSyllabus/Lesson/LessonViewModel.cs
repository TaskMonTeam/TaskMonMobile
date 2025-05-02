using AdminService.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class LessonViewModel : ObservableObject
    {
        private readonly ThemeViewModel _parent;
        
        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private LessonType? _type;
        
        [ObservableProperty]
        private float _studyHours;
        
        public LessonViewModel(ThemeViewModel parent)
        {
            _parent = parent;
            Title = string.Empty;
            Type = null;
            StudyHours = 0;
        }
        
        [RelayCommand]
        private void RemoveLesson()
        {
            _parent.RemoveLessonCommand.Execute(this);
        }
        
        public List<LessonType> LessonTypes => Enum.GetValues(typeof(LessonType)).Cast<LessonType>().ToList();
    }
}