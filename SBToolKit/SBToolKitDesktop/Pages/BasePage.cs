using SBToolKit.WPF.Animation;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System;
using SBToolKit.WPF.ViewModels;

namespace SBToolKit.WPF.Pages
{
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {
        private VM _viewmodel;

        public VM ViewModel
        {
            get => _viewmodel;
            set
            {
                if (_viewmodel == value)
                    return;
                _viewmodel = value;
                DataContext = _viewmodel;
            }
        }
        public PageAnimationType PageLoadAnimation { get; set; } = PageAnimationType.SlideAndFadeInFromRight;
        public PageAnimationType PageUnloadAnimation { get; set; } = PageAnimationType.SladeAndFadeOutToLeft;
        public float SlideSeconds { get; set; } = 0.9f;

        public BasePage()
        {
            if (PageLoadAnimation != PageAnimationType.None)
                Visibility = Visibility.Collapsed;

            Loaded += BasePage_Loaded;

            DataContext = new VM();
        }

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }

        private async Task AnimateIn()
        {
            if (PageLoadAnimation == PageAnimationType.None)
                return;

            switch(PageLoadAnimation)
            {
                case PageAnimationType.SlideAndFadeInFromRight:
                    await this.SlideAndFadeInFromRight(SlideSeconds);
                    break;
            }
        }
    }
}
