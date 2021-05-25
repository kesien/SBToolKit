using MvvmHelpers.Commands;
using SBToolKit.WPF.Models;
using SwitchServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SBToolKit.WPF.ViewModels
{
    public class WindowViewModel : ApplicationViewModel
    {
        #region Private members
        private Window _window;
        private Switchconnection _switchconnection;
        private int _windowRadius = 10;
        #endregion

        #region Public properties
        public int WindowMinWidth { get; set; } = 400;
        public int WindowMinHeight { get; set; } = 400;
        public int ResizeBorder { get; set; } = 6;
        public Thickness ResizeBorderThickness { get => new Thickness(ResizeBorder); }
        public int WindowRadius
        {
            get => _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set
            {
                _windowRadius = value;
            }
        }

        public CornerRadius WindowCornerRadius { get => new CornerRadius(_windowRadius); }
        #endregion
        public int TitleHeight { get; set; } = 30;
        public GridLength TitleHeightGridLength { get => new GridLength(TitleHeight + ResizeBorder); }
        public Thickness InnerContentPadding { get => new Thickness(0); }
        
        public Command MinimizeCommand { get; set; }
        public Command MaximizeCommand { get; set; }
        public Command CloseCommand { get; set; }
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {
            _window = window;
            MinimizeCommand = new Command(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new Command(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new Command(() => _window.Close());

            _window.StateChanged += (sender, e) =>
            {
                NotifyPropertyChanged(nameof(ResizeBorderThickness));
                NotifyPropertyChanged(nameof(WindowRadius));
                NotifyPropertyChanged(nameof(WindowCornerRadius));
            };
        }
        #endregion
    }
}
