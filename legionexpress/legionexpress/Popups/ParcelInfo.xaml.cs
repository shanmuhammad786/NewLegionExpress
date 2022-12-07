using legionexpress.Helpers;
using legionexpress.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class ParcelInfo : PopupPage
    {
        ParcelInfoViewModel viewModel;
        public ParcelInfo(string shipmentID)
        {
            InitializeComponent();
            Utilty.count++;
            checkcount();
            viewModel = new ParcelInfoViewModel(shipmentID);
            BindingContext = viewModel;
        }
        private void checkcount()
        {
            if(Utilty.count > 1)
            {
                PopupNavigation.Instance.PopAsync();
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<string>("1", "EnableScan");
            Utilty.count = 0;
        }
    }
}