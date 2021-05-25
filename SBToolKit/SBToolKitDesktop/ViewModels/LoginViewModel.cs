using MvvmHelpers.Commands;
using SBToolKit.WPF.Models;
using SBToolKit.WPF.Security;
using SwitchServiceLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SBToolKit.WPF.ViewModels
{
    public class LoginViewModel : ApplicationViewModel
    {
        private string _username;
        public string Username 
        { 
            get => _username; 
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }
        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                NotifyPropertyChanged(nameof(IsLoading));
            }
        }

        private Visibility _loginTextVisibility;
        public Visibility LoginTextVisibility
        {
            get => _loginTextVisibility;
            set
            {
                _loginTextVisibility = value;
                NotifyPropertyChanged(nameof(LoginTextVisibility));
            }
        }

        public AsyncCommand<object> LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncCommand<object>(Login);
        }

        private async Task Login(object parameter)
        {
            IsLoading = true;
            LoginTextVisibility = Visibility.Collapsed;
            var password = (parameter as IHavePassword).SecurePassword.Unsecure();
            try
            {
                await SwitchConnection.LoginAsync(Username, password);
                CurrentPage = ApplicationPage.Cloudlearning;
            }
            catch (InvalidUsernameOrPasswordException e)
            {
                Username = e.Message;
            }
            
            Debug.WriteLine("Invalid!!");
            IsLoading = false;
            LoginTextVisibility = Visibility.Visible;
        }
    }
}
