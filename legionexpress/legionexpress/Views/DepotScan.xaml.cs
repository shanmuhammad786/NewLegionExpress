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
    public partial class DepotScan : ContentPage
    {
        ReviewScanViewModel viewModel;
        public DepotScan()
        {
            InitializeComponent();
            viewModel = new ReviewScanViewModel();
            BindingContext = viewModel;

        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }

        private async void Exception_Clicked(object sender, EventArgs e)
        {
            var hold = await Application.Current.MainPage.DisplayAlert("Exception Consignment!", "Are you sure Exception consignment", "yes", "no");
            if (hold)
            {
                await Navigation.PushAsync(new Home());
            }
        }
    }
}