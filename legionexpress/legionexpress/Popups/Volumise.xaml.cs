using legionexpress.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Volumise : PopupPage
    {
        public Volumise()
        {
            InitializeComponent();
            BindingContext = new VolumiseViewModel();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<string>("1", "LoadShipmentData");

        }

        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as VolumiseViewModel;
            if(vm != null)
            {
                vm.TotalCubeText = CubeCalculation(vm.Length,vm.Height, e.NewTextValue);
            }
        }

        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as VolumiseViewModel;
            if (vm != null)
            {
                vm.TotalCubeText = CubeCalculation(vm.Length,e.NewTextValue, vm.Width);
            }
        }
        private void Length_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as VolumiseViewModel;
            if (vm != null)
            {
               vm.TotalCubeText = CubeCalculation(e.NewTextValue, vm.Height, vm.Width);
            }
        }
        private int CubeCalculation(string length, string height, string width)
        {
            if(string.IsNullOrEmpty(length))
            {
                length = "0";
            }
            if (string.IsNullOrEmpty(height))
            {
                height = "0";
            }
            if (string.IsNullOrEmpty(width))
            {
                width = "0";
            }
            var total = double.Parse(width) * double.Parse(height) * double.Parse(length) / 5000;
            return Convert.ToInt32(total);
        }
    }
}