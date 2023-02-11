using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class ChangeNetworkViewModel : BaseViewModel
    {
        LookUpService _service;
        private Network _selectedNetwork;
        private ShipmentService _shipmentService;
        public Network SelectedNetwork
        {
            get
            {
                return _selectedNetwork;
            }
            set
            {
                _selectedNetwork = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<Network> _networkList;
        public ObservableCollection<Network> NetworkList
        {
            get
            {
                return _networkList;
            }
            set
            {
                _networkList = value;
                NotifyPropertyChanged();
            }
        }

        public ChangeNetworkViewModel()
        {
            _service = new LookUpService();
            _shipmentService = new ShipmentService();
            GetNetworks();
        }
        #region Commands
        public ICommand ClosePopupCommand => new Command(ClosePopup);
        #endregion
        #region Methods
        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public async void ChangeNetwork()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var networkObj = new
                {
                    Id = shipmentID,
                    NetworkId = SelectedNetwork.id
                };
                var response = await _service.ChangeNetworks(networkObj);
                if (response.hasError == false)
                {

                    await PopupNavigation.Instance.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Network is Updated"));
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void GetNetworks()
        {
            try
            {
                var response = await _service.GetNetworks();

                if (response.hasError == false)
                {
                    var networkList = new ObservableCollection<Network>(response.result);
                    foreach (var item in networkList)
                    {
                        if(item.name=="Legion")
                        {
                            item.Image = "legion.jpeg";
                            item.BackgroundColor = Color.FromHex("#000000");
                        }

                        if (item.name == "UPS")
                        {
                            item.Image = "ups.png";
                            item.BackgroundColor = Color.FromHex("#000000");

                        }
                        if (item.name == "DHL")
                        {
                            item.Image = "dhl.png";
                            item.BackgroundColor = Color.FromHex("#000000");

                        }
                        if (item.name == "DXExpress")
                        {
                            item.Image = "dxe.png";
                            item.BackgroundColor = Color.FromHex("#000000");
                        }
                        if (item.name == "DXFreight")
                        {
                            item.Image = "dxf.png";
                            item.BackgroundColor = Color.FromHex("#000000");
                        }
                        if (item.name == "WorldOptions")
                        {
                            item.Image = "NetworkSelectWorldOptions.png";
                            item.BackgroundColor = Color.FromHex("#000000");
                        }
                    }
                    NetworkList = networkList;
                    GetShipmentById();

                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void GetShipmentById()
        {
            try
            {
                var shipmentId = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _shipmentService.GetShipmentById(shipmentId);
                if (result != null && result.hasError == false)
                {
                    var changedNetwork = result.result.networkChangeRequest;
                    var selectedNetwork = NetworkList.Where(x => x.name == changedNetwork).FirstOrDefault();
                    if(selectedNetwork != null)
                    {
                       // NetworkList.Remove(selectedNetwork);
                        selectedNetwork.BackgroundColor = Color.Orange;
                        //NetworkList.Add(selectedNetwork);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}

