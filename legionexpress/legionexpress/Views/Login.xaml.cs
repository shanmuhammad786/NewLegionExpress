using legionexpress.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private async void Login_Clicked(object sender, EventArgs e)
        {
                await Navigation.PushAsync(new Home());
        }
    }
}