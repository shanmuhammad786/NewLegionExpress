using legionexpress.Helpers;
using legionexpress.Models;
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
    public partial class AlreadyScanned : PopupPage
    {
        public AlreadyScanned(string lengthAlert, string instructions, ShipmentResult shipmentResult, string localPostalCode)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(lengthAlert))
            {
                LengthAlert.Text = lengthAlert;
            }
            else
            {
                LengthAlert.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(instructions))
            {
                InstructionsText.Text = instructions;
            }
            else
            {
                Instructions.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(localPostalCode))
            {
                PostalCodeText.Text = localPostalCode;
            }
            else
            {
                PostalCode.IsVisible = false;
            }
            BindingContext =new AlreadyScannedViewModel(shipmentResult);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<string>("1", "DisableScan");

            //var vm= BindingContext as AlreadyScannedViewModel;
            //vm.GetShipmentById();
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await Task.Delay(500);
                if (PopupNavigation.PopupStack.Count == 0)
                MessagingCenter.Send<string>("1", "EnableScan");

            Utilty.count = 0;

        }
    }
}