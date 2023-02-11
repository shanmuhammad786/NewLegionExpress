using legionexpress.Interfaces;
using legionexpress.Models;
using Scandit.DataCapture.Barcode.Capture.Unified;
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
    public partial class ExemptionScreen : ContentPage
    {
        public BarcodeCapture BarcodeCapture { get; private set; } = ScannerModel.Instance.BarcodeCapture;

        public ExemptionScreen(string RecognizedCode = "")
        {
            InitializeComponent();
            DisplayAlert("BarCode", RecognizedCode, "OK");
            //DependencyService.Get<Toast>().Show(RecognizedCode);
        }

        private async void ChangeNetwork_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangeNetwork());
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new NavigationPage(new CollectionHome());
            return base.OnBackButtonPressed();
        }
        private async void Weight_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeightDimensions());

        }

        private async void Hold_Tapped(object sender, EventArgs e)
        {   
            await Navigation.PushAsync(new Hold());

        }
        private async void ReportDamage_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportDamage());

        }
        private async void Release_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Release());

        }
        private async void RTS_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RTS());

        }
    }
}