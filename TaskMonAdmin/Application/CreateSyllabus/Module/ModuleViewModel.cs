using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class ModuleViewModel : ObservableObject
    {
        private readonly CreateSyllabusPageViewModel _parent;
        
        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private ObservableCollection<ThemeViewModel> _themes;
        
        public ModuleViewModel(CreateSyllabusPageViewModel parent)
        {
            _parent = parent;
            Title = string.Empty;
            Themes = new ObservableCollection<ThemeViewModel>();
        }
        
        [RelayCommand]
        private void AddTheme()
        {
            var newTheme = new ThemeViewModel(this);
            Themes.Add(newTheme);
            
            newTheme.AddLessonCommand.Execute(null);
        }
        
        [RelayCommand]
        private void RemoveTheme(ThemeViewModel theme)
        {
            if (Themes.Contains(theme))
            {
                Themes.Remove(theme);
                
                if (Themes.Count == 0)
                {
                    _parent.RemoveModuleCommand.Execute(this);
                }
            }
        }
    }
}