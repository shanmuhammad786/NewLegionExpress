using legionexpress.ViewModels;
using legionexpress.Views;
using legionexpress.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Logout : PopupPage
	{
		public Logout ()
		{
			InitializeComponent ();
            BindingContext = new LogoutViewModel();
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await AppNotificationManager.Instance.StopAsync();
            Preferences.Remove("token");
            PopupNavigation.Instance.PopAsync();
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}