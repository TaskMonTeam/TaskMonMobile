using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AdminService.Client;
using AdminService.Client.Requests;
using AdminService.Models;

namespace TaskMonAdmin.ViewModels
{
    public partial class CreateSyllabusPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _adminClient;
        
        [ObservableProperty]
        private ObservableCollection<ModuleViewModel> _modules;
        
        [ObservableProperty]
        private Guid _courseId;
        
        public CreateSyllabusPageViewModel(ITaskMonAdminClient adminClient)
        {
            _adminClient = adminClient;
            Modules = new ObservableCollection<ModuleViewModel>();
        }
        
        [RelayCommand]
        private void AddModule()
        {
            var newModule = new ModuleViewModel(this);
            Modules.Add(newModule);
            
            newModule.AddThemeCommand.Execute(null);
        }
        
        [RelayCommand]
        private void RemoveModule(ModuleViewModel module)
        {
            if (Modules.Contains(module))
            {
                Modules.Remove(module);
            }
        }
        
        [RelayCommand]
        private async Task UpdateSyllabus()
        {
            try
            {
                var modulesList = Modules.Select(m => 
                    new Module(
                        m.Title,
                        m.Themes.Select(t => 
                            new Theme(
                                t.Title,
                                t.Lessons.Select(l => 
                                    new Lesson(
                                        l.Title,
                                        l.Type ?? LessonType.Lecture,
                                        l.StudyHours
                                    )
                                ).ToList()
                            )
                        ).ToList()
                    )
                ).ToList();
                
                var request = new CreateSyllabusRequest(modulesList);
                
                await _adminClient.UpdateCourseSyllabus(CourseId, request);
                await Shell.Current.GoToAsync($"//SyllabusGroupPage?courseId={CourseId}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка при оновленні силабуса: {ex.Message}", "OK");
            }
        }
    }
}