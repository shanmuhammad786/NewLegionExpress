using legionexpress.Models;
using legionexpress.Services;
using legionexpress.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        public INavigation Navigation { get; set; }
        AuthService _service;
        private LoginModel _login;
        public LoginModel Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                NotifyPropertyChanged();
            }
        }
        private bool _isRunning;
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        public LoginViewModel(INavigation navigation)
        {
            Login = new LoginModel();
            _service = new AuthService();
            this.Navigation = navigation;
        }
        public ICommand LoginCommand => new Command(async () => await LoginCommandAsync());
        private async Task LoginCommandAsync()
        {
            this.IsRunning = true;
            var authentication = await _service.GenerateToken(Login);
            if (authentication.hasError == false)
            {
                Preferences.Set("token", authentication.result.apiKey);
                Application.Current.MainPage = new NavigationPage(new Home());
            }
            else
            {
               await Application.Current.MainPage.DisplayAlert("Error", "Invalid UserID Or Pin Number", "Ok");
            }
            this.IsRunning = false;
        }
    }
}
