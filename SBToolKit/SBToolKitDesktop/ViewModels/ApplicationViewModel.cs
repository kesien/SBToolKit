using SBToolKit.WPF.Models;
using SwitchServiceLibrary;

namespace SBToolKit.WPF.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private Switchconnection _switchconnection;
        public Switchconnection SwitchConnection 
        {
            get => _switchconnection;
            set => _switchconnection = value;
        }
        private ApplicationPage _currentPage = ApplicationPage.Cloudlearning;
        public ApplicationPage CurrentPage 
        { 
           get => _currentPage; 
           set
            {
                _currentPage = value;
                NotifyPropertyChanged(nameof(CurrentPage));
            }
        }

        public ApplicationViewModel()
        {
            _switchconnection = new();
        }
    }
}
