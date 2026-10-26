using System;
using System.Threading.Tasks;
using legionexpress.Popups;
using legionexpress.Services;
using legionexpress.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionHome : ContentPage
    {
        private DashboardViewModel ViewModel => BindingContext as DashboardViewModel;

        public CollectionHome()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
                await ViewModel.InitializeAsync();
        }

        private async void Scan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanditCollectionScanning());
        }

        private async void ExemptionScan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExemptionScan());
        }

        private async void InfoScan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoScan());
        }

        private async void DepotScan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DepotScan());
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new Logout());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await AppNotificationManager.Instance.StopAsync();
            Preferences.Remove("token");
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
