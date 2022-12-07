using legionexpress.Helpers;
using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class ParcelInfoViewModel : BaseViewModel
    {
        ShipmentService _service;
        #region properties
        private ShipmentResult _shipmentDetails;
        private bool _isRunning;
        private string _length;
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                NotifyPropertyChanged();
            }
        }
        public ShipmentResult ShipmentDetails
        {
            get
            {
                return _shipmentDetails;
            }
            set
            {
                _shipmentDetails = value;
                NotifyPropertyChanged();
            }
        }
        public string Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        public ParcelInfoViewModel(string shipmentID)
        {
            _service = new ShipmentService();
            LoadData(shipmentID);
        }
        //private async void GetScannedShipment(string networkid)
        //{
        //    try
        //    {
        //        IsRunning = true;
        //        var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
        //        var response = await _service.GetScanShipment(shipmentID);
        //        if (response != null && response.hasError == false)
        //        {
        //            if (response.result != null)
        //            {
        //                var result = response.result.ToString();
        //                if (result == "True" || result == "true")
        //                {
        //                    PopupNavigation.Instance.PushAsync(new AlreadyScanned() {BindingContext=this });
        //                   // Application.Current.MainPage.DisplayAlert("Alert", "This Parcel is already scanned", "OK");
        //                    LoadData(networkid);
        //                }
        //                else
        //                {
        //                    SetScannedShipment(networkid);
        //                }
        //            }
        //            else
        //            {
        //                SetScannedShipment(networkid);
        //            }
        //        }

        //        IsRunning = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        IsRunning = false;
        //    }
        //}
        private async void SetScannedShipment()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var obj = new SetShipmentModel()
                {
                    ids = new List<string>()
                    {
                        shipmentID
                    }
                };
                var response = await _service.SetScanShipment(obj);
                if (response != null && response.hasError == false)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        public async void LoadData(string shipmentId)
        {
            this.IsRunning = true;
            var shipmentDetails = await _service.GetShipmentById(shipmentId);
            if (shipmentDetails != null)
            {
                foreach(var item in shipmentDetails.result.consignmentItems)
                {
                    if(item.id == shipmentId)
                    {
                        item.scanItemBackground = Color.Orange;
                    }
                    else if(item.isCollectionScanned)
                    {
                        item.scanItemBackground = Color.Green;
                    }
                    else
                    {
                        item.scanItemBackground = Color.White;
                    }
                }


                var consignmmentItem = shipmentDetails.result.consignmentItems.Where(x => x.id == shipmentId).FirstOrDefault();
                Length = consignmmentItem.palletLength.ToString();
                ShipmentDetails = shipmentDetails.result;
                if (string.IsNullOrEmpty(shipmentDetails.result.notes))
                {
                    ShipmentDetails.deliveryInstruction = shipmentDetails.result.deliveryInstruction;
                }
                else
                {
                    ShipmentDetails.deliveryInstruction = shipmentDetails.result.notes;
                }
                await Utilty.SetSecureStorageValue(Utilty.HoldandReleaseKey, shipmentDetails.result.trackStatus);
                SetScannedShipment();
            }

            //if (shipment.hasError == false)
            //{

            //    Application.Current.MainPage = new NavigationPage(new Home());
            //}
            //else
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Invalid UserID Or Pin Number", "Ok");
            //}
            this.IsRunning = false;
        }
    }
}
