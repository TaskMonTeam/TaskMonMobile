using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace TaskMonMobile.ViewModels
{
    public partial class SurveyItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _title;
        
        [ObservableProperty]
        private bool _isCompleted;

        public SurveyGroupPageViewModel ParentViewModel { get; set; }

        public string CompletionStatus => IsCompleted ? "Пройдено" : "Не пройдено";

        public ICommand SelectSurveyCommand => new Command(() => 
        {
            if (!IsCompleted)
            {
                ParentViewModel.NavigateToSurveyCommand.Execute(Id);
            }
        });
    }
}