using legionexpress.Popups;
using Rg.Plugins.Popup.Services;
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
    public partial class WarehouseHome : ContentPage
    {
        public WarehouseHome()
        {
            InitializeComponent();
        }
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        //private void ClickToShowPopup_Clicked(object sender, EventArgs e)
        //{
        //    popupLayout.PopupView.IsFullScreen = true;
        //    popupLayout.IsOpen = true;
        //}


        private async void Scan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanditWarehouseScanning());
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
    }
}