using legionexpress.Models;
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
    public partial class ChangeNetwork : PopupPage
    {
        public ChangeNetwork()
        {
            InitializeComponent();
            BindingContext = new ChangeNetworkViewModel();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<string>("1", "LoadShipmentData");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var data = sender as Frame;
            var obj = data.BindingContext as Network;
            if(obj!= null)
            {
                var vm = BindingContext as ChangeNetworkViewModel;
                vm.NetworkList.Select(p=>p.BackgroundColor=Color.FromHex("#000000"));
                vm.SelectedNetwork = obj;
                obj.BackgroundColor = Color.Orange;
                vm.ChangeNetwork();

            }

        }
    }
}