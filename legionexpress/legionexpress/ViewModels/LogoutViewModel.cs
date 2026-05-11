using legionexpress.Popups;
using legionexpress.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class LogoutViewModel :BaseViewModel
    {
        public LogoutViewModel()
        {

        }
        #region Commands
        public ICommand WarehouseCommand => new Command(WareHouse);
        public ICommand CollectionCommand => new Command(Collection);
        public ICommand ColDelListCommand => new Command(ColDelList);
        public ICommand DeliveryScanCommand => new Command(DeliveryScan);
        #endregion
        #region Methods
        private async void WareHouse()
        {
           await PopupNavigation.Instance.PopAsync();
           await (Application.Current.MainPage).Navigation.PushAsync(new WarehouseHome());
        }
        private async void Collection()
        {
            await PopupNavigation.Instance.PopAsync();
            await (Application.Current.MainPage).Navigation.PushAsync(new CollectionHome());
        }
        private async void ColDelList()
        {
            await PopupNavigation.Instance.PopAsync();
            await (Application.Current.MainPage).Navigation.PushAsync(new ColDelList());
        }
        private async void DeliveryScan()
        {
            //await PopupNavigation.Instance.PopAsync();
            //await (Application.Current.MainPage).Navigation.PushAsync(new DeliveryScanning());
        }
        #endregion
    }
}
