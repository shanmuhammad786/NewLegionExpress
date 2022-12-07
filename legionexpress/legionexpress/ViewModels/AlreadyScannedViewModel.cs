using legionexpress.Helpers;
using legionexpress.Models;
using legionexpress.Popups;
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
    internal class AlreadyScannedViewModel : BaseViewModel
    {
        #region PrivateProperties
        private ShipmentService _service;
        private string _noteText;
        private string _weightText;
        private string _networkText;
        private string _volumiseText;
        private string _collectionPostCode;
        private string _parcelNumber;
        private bool _notesVisibility;
        private bool _networkVisibility;
        private bool _weightVisibility;
        private bool _volumiseVisibility;
        private bool _labelReprintedVisibility;
        private bool _amendmentVisibility;
        string _length = string.Empty;
        string _width = string.Empty;
        string _height = string.Empty;
        #endregion
        #region PublicProperties
        public string NoteText
        {
            get
            {
                return _noteText;
            }
            set
            {
                _noteText = value;
                NotifyPropertyChanged();
            }
        }
        public string WeightText
        {
            get
            {
                return _weightText;
            }
            set
            {
                _weightText = value;
                NotifyPropertyChanged();
            }
        }
        public string NetworkText
        {
            get
            {
                return _networkText;
            }
            set
            {
                _networkText = value;
                NotifyPropertyChanged();
            }
        }
        public string VolumiseText
        {
            get
            {
                return _volumiseText;
            }
            set
            {
                _volumiseText = value;
                NotifyPropertyChanged();
            }
        }

        public string CollectionPostCode
        {
            get
            {
                return _collectionPostCode;
            }
            set
            {
                _collectionPostCode = value;
                NotifyPropertyChanged();
            }
        }
        public string ParcelNumber
        {
            get
            {
                return _parcelNumber;
            }
            set
            {
                _parcelNumber = value;
                NotifyPropertyChanged();
            }
        }
        public bool NotesVisibility
        {
            get
            {
                return _notesVisibility;
            }
            set
            {
                _notesVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public bool NetworkVisibility
        {
            get
            {
                return _networkVisibility;
            }
            set
            {
                _networkVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public bool WeightVisibility
        {
            get
            {
                return _weightVisibility;
            }
            set
            {
                _weightVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public bool VolumiseVisibility
        {
            get
            {
                return _volumiseVisibility;
            }
            set
            {
                _volumiseVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public bool LabelReprintedVisibility
        {
            get
            {
                return _labelReprintedVisibility;
            }
            set
            {
                _labelReprintedVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public bool AmendmentVisibility
        {
            get
            {
                return _amendmentVisibility;
            }
            set
            {
                _amendmentVisibility = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
        public AlreadyScannedViewModel(ShipmentResult shipmentResult)
        {
            _service = new ShipmentService();
            GetShipmentById(shipmentResult);
        }
        #region Commands
        public ICommand AmendCommand => new Command(Amend);
        public ICommand ContinueScanningCommand => new Command(ContinueScanning);
        #endregion
        #region Methods
        private async void Amend()
        {
            await PopupNavigation.Instance.PopAsync();
            await PopupNavigation.Instance.PushAsync(new Amend());
        }
        private async void ContinueScanning()
        {
            await PopupNavigation.Instance.PopAsync();
           // MessagingCenter.Send<string>("1", "EnableScan");
           // Utilty.count = 0;
        }
        public async void GetShipmentById(ShipmentResult shipmentResult)
        {
            try
            {
                    CollectionPostCode = shipmentResult.deliveryPostcode;
                    ParcelNumber = shipmentResult.parcelNumber;
                    if (!string.IsNullOrEmpty(shipmentResult.notes))
                    {
                        NoteText = shipmentResult.notes;
                        NotesVisibility = true;
                    }
                    else
                    {
                        NotesVisibility = false;

                    }
                    if (!string.IsNullOrEmpty(shipmentResult.networkChangeRequest))
                    {
                        NetworkText = shipmentResult.networkChangeRequest;
                        NetworkVisibility = true;

                    }
                    else
                    {
                        NetworkVisibility = false;
                    }
                    string[] str = shipmentResult.parcelNumber.Split(' ');
                    var number = str[0];
                    var consignmmentItem = shipmentResult.consignmentItems.Where(x => x.itemNumber == number).FirstOrDefault();
                    if (consignmmentItem.changeRequestWeight == null)
                    {
                        WeightVisibility = false;
                    }
                    else
                    {
                        WeightVisibility = true;
                        WeightText = $"{consignmmentItem.changeRequestWeight}KG";
                    }

                    if (consignmmentItem.changeRequestWidth == null && consignmmentItem.changeRequestHeight == null && consignmmentItem.changeRequestLength == null)
                    {
                        VolumiseVisibility = false;
                    }
                    else
                    {
                        VolumiseVisibility = true;
                        VolumiseText = $"Length:{consignmmentItem.changeRequestLength} Height:{consignmmentItem.changeRequestHeight} Width:{consignmmentItem.changeRequestWidth}";
                    }

                    if (consignmmentItem != null && !string.IsNullOrEmpty(consignmmentItem.dimensionsChangeRequest))
                    {

                    }
                    if (shipmentResult.labelRePrintRequest == true)
                    {
                        LabelReprintedVisibility = true;
                    }
                    else
                    {
                        LabelReprintedVisibility = false;
                    }

                    if (!NotesVisibility && !LabelReprintedVisibility && !VolumiseVisibility && !WeightVisibility && !NetworkVisibility)
                    {
                        AmendmentVisibility = false;
                    }
                    else
                    {
                        AmendmentVisibility = true;
                    }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
