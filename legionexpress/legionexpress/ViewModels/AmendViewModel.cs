using legionexpress.Helpers;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class AmendViewModel : BaseViewModel
    {
        #region PrivateProperties
        ShipmentService _service;
        private string _btnText;
        private string _notes;
        private string _weight;
        private string _holdReleaseNote;
        private bool _isRunning;
        private string _amendPriceNote
;
        private Color _addNoteColor;
        private Color _networkChangeColor;
        private Color _addWeightColor;
        private Color _volumiseColor;
        private Color _reprintLabelColor;
        private Color _holdColor;
        private Color _nonDataBagColor;
        private Color _palletColor;
        private Color _residentialColor;
        private Color _lengthGreaterThanThreeColor;
        private Color _lengthLessThanThreeColor;
        private Color _amendPriceColor;
        string length = string.Empty;
        string width = string.Empty;
        string height = string.Empty;
        #endregion
        #region PublicProperties
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
        public string BtnText
        {
            get
            {
                return _btnText;
            }
            set
            {
                _btnText = value;
                NotifyPropertyChanged();
            }
        }
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
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
        public string HoldReleaseNote
        {
            get
            {
                return _holdReleaseNote;
            }
            set
            {
                _holdReleaseNote = value;
                NotifyPropertyChanged();
            }
        }
        public Color AddNoteColor
        {
            get
            {
                return _addNoteColor;
            }
            set
            {
                _addNoteColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color NetworkChangeColor
        {
            get
            {
                return _networkChangeColor;
            }
            set
            {
                _networkChangeColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color AddWeightColor
        {
            get
            {
                return _addWeightColor;
            }
            set
            {
                _addWeightColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color VolumiseColor
        {
            get
            {
                return _volumiseColor;
            }
            set
            {
                _volumiseColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color ReprintLabelColor
        {
            get
            {
                return _reprintLabelColor;
            }
            set
            {
                _reprintLabelColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color HoldColor
        {
            get
            {
                return _holdColor;
            }
            set
            {
                _holdColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color NonDataBagColor
        {
            get
            {
                return _nonDataBagColor;
            }
            set
            {
                _nonDataBagColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color PriceAmendColor
        {
            get
            {
                return _holdColor;
            }
            set
            {
                _holdColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color PalletColor
        {
            get
            {
                return _palletColor;
            }
            set
            {
                _palletColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color ResidentialColor
        {
            get
            {
                return _residentialColor;
            }
            set
            {
                _residentialColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color LengthGreaterThanThreeColor
        {
            get
            {
                return _lengthGreaterThanThreeColor;
            }
            set
            {
                _lengthGreaterThanThreeColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color LengthLessThanThreeColor
        {
            get
            {
                return _lengthLessThanThreeColor;
            }
            set
            {
                _lengthLessThanThreeColor = value;
                NotifyPropertyChanged();
            }
        }
        public Color AmendPriceColor
        {
            get
            {
                return _amendPriceColor;
            }
            set
            {
                _amendPriceColor = value;
                NotifyPropertyChanged();
            }
        }
        public string AmendPriceNote
        {
            get
            {
                return _amendPriceNote;
            }
            set
            {
                _amendPriceNote = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        public AmendViewModel()
        {
            _service = new ShipmentService();
            //var status = Preferences.Get("TrackStatus", "default_value");
            //var key = Utilty.GetSecureStorageValueFor(Utilty.HoldandReleaseKey);
            //if (key.Result == null)
            //{
            //    BtnText = "Hold";

            //}
            //else if (key.Result.Contains("Hold"))
            //{
            //    BtnText = "Release";
            //}
            //else
            //{
            //    BtnText = "Hold";
            //}
            MessagingCenter.Subscribe<string>(this, "LoadShipmentData", (sender) =>
            {
                GetShipmentById();

            });
        }
        #region Commands
        public ICommand ReprintLabelCommand => new Command(ReprintLabel);
        public ICommand ChangeNetworkCommand => new Command(ChangeNetwork);
        public ICommand HoldAndReleaseCommand => new Command(HoldandRelease);
        public ICommand VolumiseCommand => new Command(Volumise);
        public ICommand AddNoteCommand => new Command(AddNote);
        public ICommand ClosePopupCommand => new Command(ClosePopup);
        public ICommand SubmitNoteCommand => new Command(SubmitNote);
        public ICommand AddWeightCommand => new Command(AddWeight);
        public ICommand PriceAmendCommand => new Command(PriceAmend);
        public ICommand PalletCommand => new Command(Pallet);
        public ICommand Length1m3mCommand => new Command(Length1m3m);
        public ICommand Length3mCommand => new Command(Length3m);
        public ICommand ResidentialCommand => new Command(Residential);
        public ICommand NonDataBagCommand => new Command(NonDataBag);
        public ICommand SubmitWeightCommand => new Command(SubmitWeight);
        public ICommand SubmitHoldReleaseNoteCommand => new Command(SubmitHoldReleaseNote);
        public ICommand SubmitAmendPriceCommand => new Command(SubmitAmendPrice);
        #endregion
        #region Methods
        private async void PriceAmend()
        {
            await PopupNavigation.Instance.PushAsync(new AmendPriceNote() { BindingContext = this });
        }
        private async void SubmitAmendPrice()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var amendPrice = new
                {
                    Id = shipmentID,
                    Notes = AmendPriceNote
                };
                var result = await _service.AmendPrice(amendPrice);
                if (result.hasError == false)
                {
                    await PopupNavigation.Instance.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Price is Amend Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void NonDataBag()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                DimensionUpdateDto dimensionUpdateDto = new DimensionUpdateDto()
                {
                    Weight = 6,
                    Height = 0,
                    Length = 0,
                    Width = 0,
                    Id = shipmentID
                };
                var result = await _service.UpdateDimensions(dimensionUpdateDto);
                if (result.hasError == false)
                {

                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Non Data Bag is Updated Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void Pallet()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _service.PalletRerquest(shipmentID);
                if (result.hasError == false)
                {

                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Pallet is Updated Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void Length1m3m()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _service.Length1m3m(shipmentID);
                if (result.hasError == false)
                {

                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Length is Requested Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void Length3m()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _service.Length3m(shipmentID);
                if (result.hasError == false)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Length is Requested Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void Residential()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var result = await _service.Residential(shipmentID);
                if (result.hasError == false)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Residential is Updated Successfully"));
                    GetShipmentById();
                }
            }
            catch (Exception)
            {

            }
        }
        private async void AddWeight()
        {
            await PopupNavigation.Instance.PushAsync(new AddWeight() { BindingContext = this });
        }

        private async void AddNote()
        {
            await PopupNavigation.Instance.PushAsync(new AddNote() { BindingContext = this });
        }
        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async void SubmitWeight()
        {
            try
            {
                IsRunning = true;
                if (string.IsNullOrEmpty(Weight))
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Weight cannot be empty", "OK");
                    IsRunning = false;

                }
                else
                {
                    var obj = new DimensionUpdateDto();
                    var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                    obj.Id = shipmentID;
                    obj.Weight = Convert.ToInt32(Weight);
                    if (!string.IsNullOrEmpty(width))
                    {
                        obj.Width = Convert.ToInt32(width);
                    }
                    if (!string.IsNullOrEmpty(height))
                    {
                        obj.Height = Convert.ToInt32(height);
                    }
                    if (!string.IsNullOrEmpty(length))
                    {
                        obj.Length = Convert.ToInt32(length);
                    }
                    var response = await _service.UpdateDimensions(obj);
                    if (response.hasError == false)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Weight is Updated Successfully", "OK");
                        await PopupNavigation.Instance.PopAsync();
                    }
                }

                IsRunning = false;

            }
            catch (Exception ex)
            {
                IsRunning = false;

            }
        }
        private async void SubmitNote()
        {
            try
            {
                IsRunning = true;

                if (string.IsNullOrEmpty(Notes))
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Notes cannot be empty"));
                    IsRunning = false;

                }
                else
                {
                    var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                    var obj = new
                    {
                        id = shipmentID,
                        notes = Notes
                    };
                    var response = await _service.AddNote(obj);
                    if (response != null && response.hasError == false)
                    {
                        await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Notes is Submitted Successfully"));
                        PopupNavigation.Instance.PopAsync();
                    }
                }
                IsRunning = true;

            }
            catch (Exception ex)
            {
                IsRunning = false;
            }
        }
        private async void ChangeNetwork()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new ChangeNetwork());
            }
            catch (Exception ex)
            {

            }
        }
        private async void Volumise()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new Volumise());
            }
            catch (Exception ex)
            {

            }
        }
        private async void HoldandRelease()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new HoldReleaseNote() { BindingContext = this });
            }
            catch (Exception ex)
            {

            }
        }
        private async void SubmitHoldReleaseNote()
        {
            if (BtnText == "Hold")
            {
                HoldShipment();
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                ReleaseShipment();
                await PopupNavigation.Instance.PopAsync();
            }
        }
        private async void ReprintLabel()
        {
            try
            {
                var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                var response = await _service.LabelReprint(shipmentID);
                if (response.hasError == false)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Alert", "Lebel is Reprinted"));
                    GetShipmentById();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void HoldShipment()
        {
            var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
            var response = await _service.HoldShipment(HoldReleaseNote, shipmentID);
            if (response.hasError == false)
            {
                BtnText = "Release";
                await Utilty.SetSecureStorageValue(Utilty.HoldandReleaseKey, "Hold");
            }
        }
        private async void ReleaseShipment()
        {
            var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
            var response = await _service.ReleaseShipment(HoldReleaseNote, shipmentID);
            if (response.hasError == false)
            {
                BtnText = "Hold";
                await Utilty.SetSecureStorageValue(Utilty.HoldandReleaseKey, "Release");

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
                    if (string.IsNullOrEmpty(result.result.notes))
                    {
                        Notes = result.result.deliveryInstruction;
                    }
                    else
                    {
                        Notes = result.result.notes;
                    }
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        if (result.result.statusChangeRequest != null)
                        {
                            string[] holdRelease = result.result.statusChangeRequest.Split('|');
                            if (holdRelease.Length > 1)
                            {

                                HoldReleaseNote = holdRelease[1];
                            }
                        }

                        if (!string.IsNullOrEmpty(result.result.notes))
                        {
                            //if (result.result.notes.ToLower() == "pallet")
                            //{
                            //    PalletColor = Color.Orange;
                            //}
                            //else
                            //{
                            //    PalletColor = Color.LightGray;
                            //}
                            AddNoteColor = Color.Orange;
                        }
                        else
                        {
                            AddNoteColor = Color.LightGray;
                            // PalletColor = Color.LightGray;
                        }
                    }

                    if (result.result.labelRePrintRequest == true)
                    {
                        ReprintLabelColor = Color.Orange;
                    }
                    else
                    {
                        ReprintLabelColor = Color.LightGray;
                    }
                    if (result.result.statusChangeRequest != null && !string.IsNullOrEmpty(result.result.statusChangeRequest))
                    {
                        if (result.result.statusChangeRequest.Contains("Release"))
                        {
                            BtnText = "Hold";
                            HoldColor = Color.Orange;
                        }
                        else if (result.result.statusChangeRequest.Contains("Hold"))
                        {
                            BtnText = "Release";
                            HoldColor = Color.Orange;
                        }
                    }
                    else
                    {
                        BtnText = "Hold";
                        HoldColor = Color.LightGray;
                    }

                    if (result.result.networkChangeRequest == null || string.IsNullOrEmpty(result.result.networkChangeRequest))
                    {
                        NetworkChangeColor = Color.LightGray;
                    }
                    else
                    {
                        NetworkChangeColor = Color.Orange;
                    }

                    if (result.result.palletRequest != null && (bool)result.result.palletRequest)
                    {
                        PalletColor = Color.Orange;
                    }
                    else
                    {
                        PalletColor = Color.LightGray;
                    }

                    if (result.result.residentialRequest != null && (bool)result.result.residentialRequest)
                    {
                        ResidentialColor = Color.Orange;
                    }
                    else
                    {
                        ResidentialColor = Color.LightGray;
                    }

                    if (result.result.lengthGreaterThanThreeRequest != null && (bool)result.result.lengthGreaterThanThreeRequest)
                    {
                        LengthGreaterThanThreeColor = Color.Orange;
                    }
                    else
                    {
                        LengthGreaterThanThreeColor = Color.LightGray;
                    }

                    if (result.result.lengthLessThanThreeRequest != null && (bool)result.result.lengthLessThanThreeRequest)
                    {
                        LengthLessThanThreeColor = Color.Orange;
                    }
                    else
                    {
                        LengthLessThanThreeColor = Color.LightGray;
                    }
                    if(result.result.priceAmendRequest != null && (bool)result.result.priceAmendRequest)
                    {
                        PriceAmendColor = Color.Orange;
                    }
                    else
                    {
                        PriceAmendColor = Color.LightGray;
                    }
                    if(!string.IsNullOrEmpty(result.result.notesPriceAmend))
                    {
                        AmendPriceNote = result.result.notesPriceAmend;
                    }

                    var consignmmentItem = result.result.consignmentItems.Where(x => x.id == shipmentId).FirstOrDefault();
                    if (consignmmentItem != null)
                    {
                        if (consignmmentItem.changeRequestWeight == null)
                        {
                            AddWeightColor = Color.LightGray;
                            NonDataBagColor = Color.LightGray;
                            Weight = consignmmentItem.changeRequestWeight.ToString();
                        }
                        else
                        {
                            AddWeightColor = Color.Orange;
                            Weight = consignmmentItem.changeRequestWeight.ToString();
                            if (consignmmentItem.changeRequestWeight == 6)
                            {
                                NonDataBagColor = Color.Orange;
                            }
                            else
                            {
                                NonDataBagColor = Color.LightGray;
                            }

                        }
                        if (consignmmentItem.changeRequestHeight == null && consignmmentItem.changeRequestWidth == null && consignmmentItem.changeRequestLength == null)
                        {
                            VolumiseColor = Color.LightGray;
                            length = consignmmentItem.changeRequestLength.ToString();
                            width = consignmmentItem.changeRequestWidth.ToString();
                            height = consignmmentItem.changeRequestHeight.ToString();
                        }
                        else
                        {
                            VolumiseColor = Color.Orange;
                            length = consignmmentItem.changeRequestLength.ToString();
                            width = consignmmentItem.changeRequestWidth.ToString();
                            height = consignmmentItem.changeRequestHeight.ToString();
                        }
                    }
                    else
                    {
                        VolumiseColor = Color.LightGray;
                        AddWeightColor = Color.LightGray;
                    }
                    //if (consignmmentItem != null && !string.IsNullOrEmpty(consignmmentItem.dimensionsChangeRequest))
                    //{
                    //    var strList = consignmmentItem.dimensionsChangeRequest.Split(' ').ToList();
                    //    length = strList[4];
                    //    width = strList[7];
                    //    height = strList[10];
                    //    Weight = strList[13];

                    //    //if (consignmmentItem.dimensionsChangeRequest.Contains("Weight= 0"))
                    //    //{
                    //    //    AddWeightColor = Color.LightGrey;
                    //    //}
                    //    //else
                    //    //{
                    //    //    AddWeightColor = Color.Orange;
                    //    //}
                    //    //if (consignmmentItem.dimensionsChangeRequest.Contains("Length= 0 Width= 0 Height= 0"))
                    //    //{
                    //    //    VolumiseColor = Color.LightGrey;
                    //    //}
                    //    //else
                    //    //{
                    //    //    VolumiseColor = Color.Orange;
                    //    //}
                    //}
                    //else
                    //{
                    //    Weight = result.result.weight.ToString();
                    //    length = consignmmentItem.length.ToString();
                    //    width = consignmmentItem.width.ToString();
                    //    height = consignmmentItem.height.ToString();

                    //    VolumiseColor = Color.LightGrey;
                    //    AddWeightColor = Color.LightGrey;
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
