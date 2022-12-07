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
    public partial class Amend : PopupPage
    {
        public Amend()
        {
            InitializeComponent();
            BindingContext = new AmendViewModel();
        }
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as AmendViewModel;
            MessagingCenter.Send<string>("1", "DisableScan");
            vm.GetShipmentById();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<string>("1", "EnableScan");
            Utilty.count = 0;
        }

        //protected override Task OnAppearingAnimationEndAsync()
        //{
        //    return Content.FadeTo(0.5);
        //}

        //protected override Task OnDisappearingAnimationBeginAsync()
        //{
        //    return Content.FadeTo(1);
        //}
    }
}