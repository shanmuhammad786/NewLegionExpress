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
        ShipmentService _service;
        private string _btnText;
        private string _notes;
        private string _weight;
        private string _holdReleaseNote;
        private bool _isRunning;
        private Color _addNoteColor;
        private Color _networkChangeColor;
        private Color _addWeightColor;
        private Color _volumiseColor;
        private Color _reprintLabelColor;
        private Color _holdColor;
        string length = string.Empty;
        string width = string.Empty;
        string height = string.Empty;
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
        public ICommand ReprintLabelCommand => new Command(ReprintLabel);
        public ICommand ChangeNetworkCommand => new Command(ChangeNetwork);
        public ICommand HoldAndReleaseCommand => new Command(HoldandRelease);
        public ICommand VolumiseCommand => new Command(Volumise);
        public ICommand AddNoteCommand => new Command(AddNote);
        public ICommand ClosePopupCommand => new Command(ClosePopup);
        public ICommand SubmitNoteCommand => new Command(SubmitNote);
        public ICommand AddWeightCommand => new Command(AddWeight);
        public ICommand SubmitWeightCommand => new Command(SubmitWeight);
        public ICommand SubmitHoldReleaseNoteCommand => new Command(SubmitHoldReleaseNote);

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
                    await Application.Current.MainPage.DisplayAlert("Alert", "Notes cannot be empty", "OK");
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
                        await Application.Current.MainPage.DisplayAlert("Alert", "Notes is Submitted Successfully", "OK");
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
                    await Application.Current.MainPage.DisplayAlert("Alert", "Lebel is Reprinted", "OK");
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
                    if(string.IsNullOrEmpty(result.result.notes))
                    {
                        Notes = result.result.deliveryInstruction;
                    }
                    else
                    {
                        Notes = result.result.notes;
                    }
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        if(result.result.statusChangeRequest != null)
                        {
                            string[] holdRelease = result.result.statusChangeRequest.Split('|');
                            if (holdRelease.Length > 1)
                            {

                                HoldReleaseNote = holdRelease[1];
                            }
                        }
                       
                        if (!string.IsNullOrEmpty(result.result.notes))
                        {
                            AddNoteColor = Color.Orange;
                        }
                        else
                        {
                            AddNoteColor = Color.LightGray;
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

                    var consignmmentItem = result.result.consignmentItems.Where(x => x.id == shipmentId).FirstOrDefault();
                    if(consignmmentItem != null)
                    {
                        if(consignmmentItem.changeRequestWeight == null)
                        {
                            AddWeightColor = Color.LightGray;
                            Weight = consignmmentItem.changeRequestWeight.ToString();
                        }
                        else
                        {
                            AddWeightColor = Color.Orange;
                            Weight = consignmmentItem.changeRequestWeight.ToString();

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
                    //    //    AddWeightColor = Color.LightGray;
                    //    //}
                    //    //else
                    //    //{
                    //    //    AddWeightColor = Color.Orange;
                    //    //}
                    //    //if (consignmmentItem.dimensionsChangeRequest.Contains("Length= 0 Width= 0 Height= 0"))
                    //    //{
                    //    //    VolumiseColor = Color.LightGray;
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

                    //    VolumiseColor = Color.LightGray;
                    //    AddWeightColor = Color.LightGray;
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
