using legionexpress.Helpers;
using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using legionexpress.Views;
using Rg.Plugins.Popup.Services;
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
        private string _pin1;
        private string _pin2;
        private string _pin3;
        private string _pin4;
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
        public string Pin1
        {
            get
            {
                return _pin1;
            }
            set
            {
                _pin1 = value;
                NotifyPropertyChanged();
            }
        }
        public string Pin2
        {
            get
            {
                return _pin2;
            }
            set
            {
                _pin2 = value;
                NotifyPropertyChanged();
            }
        }
        public string Pin3
        {
            get
            {
                return _pin3;
            }
            set
            {
                _pin3 = value;
                NotifyPropertyChanged();
            }
        }
        public string Pin4
        {
            get
            {
                return _pin4;
            }
            set
            {
                _pin4 = value;
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
            Utilty.username = Login.UserID;
            Login.Pin = Pin1 + Pin2 + Pin3 + Pin4;
            Preferences.Set("Username", Login.UserID);
            if (Login != null && !string.IsNullOrEmpty(Login.UserID) && !string.IsNullOrEmpty(Login.Pin))
            {
                var authentication = await _service.GenerateToken(Login);
                if (authentication.hasError == false)
                {
                    Preferences.Set("token", authentication.result.apiKey);
                    Application.Current.MainPage = new NavigationPage(new CollectionHome());
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Error", "Invalid UserID Or Pin Number"));
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "All Fields are Required."));
            }

            this.IsRunning = false;
        }
    }
}
