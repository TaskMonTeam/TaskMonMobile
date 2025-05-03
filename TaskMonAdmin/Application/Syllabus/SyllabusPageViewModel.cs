using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using AdminService.Client;
using AdminService.Models;
using CommunityToolkit.Mvvm.Input;

namespace TaskMonAdmin.ViewModels
{
    public partial class SyllabusPageViewModel : ObservableObject
    {
        private readonly ITaskMonAdminClient _syllabusesClient;
    
        [ObservableProperty]
        private Guid _id;
        
        [ObservableProperty]
        private Guid _courseId;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private DateTime _publishTime;
        
        [ObservableProperty]
        private DateTime? _archiveTime;

        [ObservableProperty]
        private ObservableCollection<SyllabusModuleViewModel> _modules;
        
        [ObservableProperty]
        private bool _isRefreshing;
        
        [ObservableProperty]
        private bool _useCurrentSyllabus;
        
        public string PublishTimeDisplay => $"Опубліковано: {PublishTime:dd.MM.yyyy HH:mm}";
        public string ArchiveTimeDisplay => $"Архівується: {ArchiveTime:dd.MM.yyyy HH:mm}";

        public SyllabusPageViewModel(ITaskMonAdminClient syllabusesClient)
        {
            _syllabusesClient = syllabusesClient;
            Modules = new ObservableCollection<SyllabusModuleViewModel>();
        }
        
        [RelayCommand]
        private async Task RefreshData()
        {
            IsRefreshing = true;
            try
            {
                await LoadSyllabusDataAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        public async Task LoadSyllabusDataAsync()
        {
            try
            {
                Syllabus syllabus;
                
                if (UseCurrentSyllabus)
                {
                    syllabus = await _syllabusesClient.GetCourseCurrentSyllabusAsync(CourseId);
                    Id = syllabus.Id;
                }
                else
                {
                    syllabus = await _syllabusesClient.GetCourseSyllabusAsync(CourseId, Id);
                }
                
                LoadFromModel(syllabus);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Помилка", $"Виникла помилка: {ex.Message}", "OK");
            }
        }

        private void LoadFromModel(Syllabus syllabus)
        {
            Id = syllabus.Id;
            Title = syllabus.Title;
            PublishTime = syllabus.PublishTime;
            ArchiveTime = syllabus.ArchiveTime;
            OnPropertyChanged(nameof(PublishTimeDisplay));
            OnPropertyChanged(nameof(ArchiveTimeDisplay));
            
            Modules.Clear();
            
            foreach (var module in syllabus.Modules)
            {
                Modules.Add(SyllabusModuleViewModel.FromModel(module));
            }
        }
    }
}