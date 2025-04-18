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

        public SurveyGroupPageViewModel ParentViewModel { get; set; }

        public ICommand SelectSurveyCommand => new Command(() => ParentViewModel.NavigateToSurveyCommand.Execute(Id));
    }
}