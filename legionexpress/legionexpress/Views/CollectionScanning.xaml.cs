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
    public partial class CollectionScanning : ContentPage
    {
        CollectionScanningViewModel viewModel;
        public CollectionScanning()
        {
            InitializeComponent();
            viewModel = new CollectionScanningViewModel();
            BindingContext = viewModel;
        }

        private async void ReviewScan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScanditCollectionScanning());
        }
    }
}