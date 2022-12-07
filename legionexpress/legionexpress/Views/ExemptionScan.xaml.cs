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
    public partial class ExemptionScan : ContentPage
    {
        ExemptionScanViewModel viewModel;
        public ExemptionScan()
        {
            InitializeComponent();
            viewModel = new ExemptionScanViewModel(Navigation);
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //var code = viewModel.RecognizedCode;
            //if (code != null && code != "")
            //{
            //    viewModel.RecognizedCode = "";
            //    await Navigation.PushAsync(new ExemptionScreen(code));
            //}
        }
        private async void ParcelScan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanditExceptionScan());
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }
    }
}