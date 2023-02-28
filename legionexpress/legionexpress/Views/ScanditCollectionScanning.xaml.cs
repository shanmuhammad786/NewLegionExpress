using legionexpress.Popups;
using legionexpress.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanditCollectionScanning : ContentPage
    {
        ScanditCollectionScanningViewModel viewModel;

        public ScanditCollectionScanning()
        {
            InitializeComponent();
            viewModel = new ScanditCollectionScanningViewModel(Navigation);
            BindingContext = viewModel;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
           // this.viewModel.InitializeScanner();
            await this.viewModel.OnResumeAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.viewModel.OnSleep();
            this.viewModel.BarCodeDispose();
        }

        private async void OnCancel_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CollectionHome());
            await Navigation.PopAsync();
        }
        private async void onAmendClicked(object sender, EventArgs e)
        {
            var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
            if (shipmentID != "default_value")
            {
                await PopupNavigation.Instance.PushAsync(new Amend());
            }
            else
            {
                await DisplayAlert("Alert", "Please Scan Document First", "OK");
            }
        }

        private async void StopScanning_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReviewScan(this.viewModel.BarCode));
        }
    }
}