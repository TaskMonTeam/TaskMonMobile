using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class ThemeViewModel : ObservableObject
    {
        private readonly ModuleViewModel _parent;
        
        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private ObservableCollection<LessonViewModel> _lessons;
        
        public ThemeViewModel(ModuleViewModel parent)
        {
            _parent = parent;
            Title = string.Empty;
            Lessons = new ObservableCollection<LessonViewModel>();
        }
        
        [RelayCommand]
        private void AddLesson()
        {
            Lessons.Add(new LessonViewModel(this));
        }
        
        [RelayCommand]
        private void RemoveLesson(LessonViewModel lesson)
        {
            if (Lessons.Contains(lesson))
            {
                Lessons.Remove(lesson);
                
                if (Lessons.Count == 0)
                {
                    _parent.RemoveThemeCommand.Execute(this);
                }
            }
        }
    }
}