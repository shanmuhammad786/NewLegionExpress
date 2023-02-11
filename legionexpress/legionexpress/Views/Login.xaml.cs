using legionexpress.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        LoginViewModel viewModel;
        public Login()
        {
            InitializeComponent();
            viewModel = new LoginViewModel(Navigation);
            BindingContext = viewModel;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            UserEntry.Focus();
        }
        private async void Login_Clicked(object sender, EventArgs e)
        {
                await Navigation.PushAsync(new CollectionHome());
        }

        private void Pin1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if(vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Pin1))
                {
                    if(vm.Pin1.Length > 1)
                    {
                        vm.Pin1 = vm.Pin1.Substring(0, 1);
                    }
                    Pin2.Focus();
                    // Pin1.Unfocus();
                }
                vm.Pin1 = e.NewTextValue;
            }

        }

        private void Pin2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Pin2))
                {
                    if (vm.Pin2.Length > 1)
                    {
                        vm.Pin2 = vm.Pin2.Substring(0, 1);
                    }
                    Pin3.Focus();
                    // Pin2.Unfocus();
                }
                else
                {
                    Pin1.Focus();
                }
                vm.Pin2 = e.NewTextValue;
            }

        }

        private void Pin3_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if (!string.IsNullOrEmpty(vm.Pin3))
            {
                if (vm.Pin3.Length > 1)
                {
                    vm.Pin3 = vm.Pin3.Substring(0, 1);
                }
                Pin4.Focus();
            }
            else
            {
                Pin2.Focus();
                vm.Pin3 = e.NewTextValue;
            }
        }
        private void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if(vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Pin4))
                {
                    if (Pin4.Text.Length > 1)
                    {
                        var oldCharacter = Pin4.Text.Substring(0, 1);
                        if (vm != null)
                        {
                            vm.Pin4 = oldCharacter;
                        }
                    }
                    else
                    {
                        vm.Pin4 = e.NewTextValue;
                    }
                }
                else
                {
                    Pin3.Focus();
                    vm.Pin4 = e.NewTextValue;
                }
            }
        }
    }
}