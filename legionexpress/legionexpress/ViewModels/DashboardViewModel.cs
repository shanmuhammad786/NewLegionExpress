using System;
using legionexpress.Popups;
using legionexpress.Views;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
	public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel()
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
            await (Application.Current.MainPage).Navigation.PushAsync(new WarehouseHome());
        }
        private async void Collection()
        {
            await (Application.Current.MainPage).Navigation.PushAsync(new ScanditCollectionScanning());
        }
        private async void ColDelList()
        {
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
