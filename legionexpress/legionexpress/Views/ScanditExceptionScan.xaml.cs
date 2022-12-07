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
    public partial class ScanditExceptionScan : ContentPage
    {
        ScanditExceptionScanViewModel viewModel;
        public ScanditExceptionScan()
        {
            InitializeComponent();
            viewModel = new ScanditExceptionScanViewModel(Navigation);
            BindingContext = viewModel;
            

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _ = this.viewModel.OnResumeAsync();
            MessagingCenter.Subscribe<string>(this, "AddNew", async (sender) =>
            {
                Device.InvokeOnMainThreadAsync(async() =>
                {
                   await Navigation.PushAsync(new ExemptionScreen());
                });
                
            });
           
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.viewModel.OnSleep();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new ExemptionScreen());

        }
    }
}