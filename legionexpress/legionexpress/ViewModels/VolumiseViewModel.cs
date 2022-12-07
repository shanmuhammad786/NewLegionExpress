using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class VolumiseViewModel : BaseViewModel
    {
        private readonly ShipmentService _service;
        #region PrivateProperties
        private string _length;
        private string _width;
        private string _height;
        private string _weight;
        private string _dimensionChangeLabel;
        private int _totalCubeText;
        #endregion
        #region PublicProperties
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
        public string Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                NotifyPropertyChanged();
            }
        }
        public string Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                NotifyPropertyChanged();
            }
        }
        public string Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                NotifyPropertyChanged();
            }
        }

        public string DimentionChangeLabel
        {
            get
            {
                return _dimensionChangeLabel;
            }
            set
            {
                _dimensionChangeLabel = value;
                NotifyPropertyChanged();
            }
        }
        public int TotalCubeText
        {
            get
            {
                return _totalCubeText;
            }
            set
            {
                _totalCubeText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        public VolumiseViewModel()
        {
            _service = new ShipmentService();
            GetShipmentById();
        }
        public ICommand DimentionsCommand => new Command(Dimentions);
        private async void Dimentions()
        {
            try
            {
                var obj = new DimensionUpdateDto();
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                obj.Id = shipmentID;
                if (string.IsNullOrEmpty(Length))
                {
                    Length = "0";
                }
                if (string.IsNullOrEmpty(Width))
                {
                    Width = "0";
                }
                if (string.IsNullOrEmpty(Height))
                {
                    Height = "0";
                }
                obj.Length = double.Parse(Length);
                obj.Width = double.Parse(Width);
                obj.Height = double.Parse(Height);
                if (!string.IsNullOrEmpty(Weight.ToString()))
                {
                    obj.Weight = double.Parse(Weight);
                }
                var response = await _service.UpdateDimensions(obj);
                if (response.hasError == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Dimentions are Updated Successfully", "OK");
                    await PopupNavigation.Instance.PopAsync();
                }

            }
            catch (Exception ex)
            {

            }
        }
        public async void GetShipmentById()
        {
            try
            {
                var shipmentId = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _service.GetShipmentById(shipmentId);
                if (result != null && result.hasError == false)
                {
                    var consignmmentItem = result.result.consignmentItems.Where(x => x.id == shipmentId).FirstOrDefault();
                    if (consignmmentItem != null)
                    {
                        if(consignmmentItem.changeRequestLength == 0)
                        {
                            Length = String.Empty;
                        }
                        else
                        {
                            Length = consignmmentItem.changeRequestLength.ToString();
                        }
                        if (consignmmentItem.changeRequestHeight == 0)
                        {
                            Height = String.Empty;
                        }
                        else
                        {
                            Height = consignmmentItem.changeRequestHeight.ToString();

                        }
                        if (consignmmentItem.changeRequestWidth == 0)
                        {
                            Width = String.Empty;
                        }
                        else
                        {
                            Width = consignmmentItem.changeRequestWidth.ToString();

                        }

                        Weight = consignmmentItem.changeRequestWeight.ToString();
                    }

                    //if (!string.IsNullOrEmpty(consignmmentItem.dimensionsChangeRequest))
                    //{
                    //    var strList = consignmmentItem.dimensionsChangeRequest.Split(' ').ToList();
                    //    Length = strList[4];
                    //    if (Length == "0.00")
                    //    {
                    //        Length = String.Empty;
                    //    }
                    //    Width = strList[7];
                    //    if (Width == "0.00")
                    //    {
                    //        Width = String.Empty;
                    //    }
                    //    Height = strList[10];
                    //    if (Height == "0.00")
                    //    {
                    //        Height = String.Empty;
                    //    }
                    //    Weight = strList[13];
                    //}
                    //else
                    //{
                    //    Weight = result.result.weight.ToString();
                    //    Length = consignmmentItem.length.ToString();
                    //    Width = consignmmentItem.width.ToString();
                    //    Height = consignmmentItem.height.ToString();
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
