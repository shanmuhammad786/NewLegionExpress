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
    public partial class ReviewScan : ContentPage
    {
        ReviewScanViewModel viewModel;
        public ReviewScan(string barcode)
        {
            InitializeComponent();
            string combindedString = string.Join(",", barcode);
            DisplayAlert("BarCode", combindedString, "OK");
            viewModel = new ReviewScanViewModel();
            BindingContext = viewModel;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConsignmentDetail());
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanExemptionScreen());

        }
    }
}