using System.ComponentModel;
using System.Runtime.CompilerServices;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using System.Linq;
using legionexpress.Models;
using ScanditStyle = Scandit.DataCapture.Core.UI.Style.Unified;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System;
using Rg.Plugins.Popup.Services;
using legionexpress.Popups;
using System.Windows.Input;
using legionexpress.Services;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.UI.Unified;
using legionexpress.Helpers;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

namespace legionexpress.ViewModels
{
    public class ScanditWarehouseScanningViewModel :BaseViewModel, INotifyPropertyChanged, IBarcodeCaptureListener
    {
        #region PrivateProperties
        private static readonly ScannerModel settings = ScannerModel.Instance;
        private IViewfinder viewfinder;
        private ScanditStyle.Brush highlightingBrush;
        ShipmentService _service;
        private bool _isLoading;
        private bool _amendmendVisibility;
        private string _warehouseScannedCount;
        #endregion
        #region PublicProperties
        public bool AmendmendVisibility
        {
            get
            {
                return _amendmendVisibility;
            }
            set
            {
                _amendmendVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public string WarehouseScannedCount
        {
            get
            {
                return _warehouseScannedCount;
            }
            set
            {
                _warehouseScannedCount = value;
                OnPropertyChanged("WarehouseScannedCount");
            }
        }
        #endregion

        public Camera Camera { get; private set; } = ScannerModel.Instance.CurrentCamera;

        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; } = ScannerModel.Instance.BarcodeCaptureSettings;
        public DataCaptureContext DataCaptureContext { get; private set; } = ScannerModel.Instance.DataCaptureContext;
        //DataCaptureView dataCaptureView { get; private set; } = ScannerModel.Instance.DataCaptureContext


        public BarcodeCapture BarcodeCapture { get; private set; } = ScannerModel.Instance.BarcodeCapture;
        public ScanditStyle.Brush HighlightingBrush
        {
            get { return this.highlightingBrush; }
            set
            {
                this.highlightingBrush = value;
                this.OnPropertyChanged(nameof(HighlightingBrush));
            }
        }
        public IViewfinder Viewfinder
        {
            get { return this.viewfinder; }
            set
            {
                this.viewfinder = value;
                this.OnPropertyChanged(nameof(Viewfinder));
            }
        }

        public MarginsWithUnit ScanAreaMargins
        {
            get
            {
                return settings.ScanAreaMargins;
            }
        }


        private string _barcode;
        public string BarCode
        {
            get
            {
                return _barcode;
            }
            set
            {
                _barcode = value;
                this.OnPropertyChanged(nameof(BarCode));
            }
        }
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                this.OnPropertyChanged(nameof(IsLoading));
            }
        }

        public INavigation Navigation { get; set; }
        public ScanditWarehouseScanningViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            _service = new ShipmentService();
            AmendmendVisibility = true;
            GetScannedCount();
            this.InitializeScanner();
        }
        #region Commands
        public ICommand CloseCommand => new Command(Close);
        public ICommand InstructionsCloseCommand => new Command(InstructionsClose);
        public ICommand StopScanningCommand => new Command(StopScanning);
        #endregion
        private async void GetScannedCount()
        {
            var count = Preferences.Get("WarehouseScannedCount", "0");
            if (count != null)
            {
                WarehouseScannedCount = count;
            }

        }
        private void Close()
        {
            PopupNavigation.Instance.PopAsync();
            if (PopupNavigation.PopupStack.Count == 1)
            {
                MessagingCenter.Send<string>("1", "EnableScan");
            }
        }
        private void InstructionsClose()
        {
            PopupNavigation.Instance.PopAsync();

        }
        private void StopScanning()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void InitializeScanner()
        {
            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.BarcodeCapture.AddListener(this);
            //BarcodeCapture.ApplySettingsAsync();


            // Rectangular viewfinder with an embedded Scandit logo.
            // The rectangular viewfinder is displayed when the recognition is active and hidden when it is not.
            //this.Viewfinder = new  RectangularViewfinder(RectangularViewfinderStyle.Legacy, RectangularViewfinderLineStyle.Light);
            this.Viewfinder = new LaserlineViewfinder(LaserlineViewfinderStyle.Legacy);

            // Adjust the overlay's barcode highlighting to match the new viewfinder styles and improve the visibility of feedback.
            // With 6.10 we will introduce this visual treatment as a new style for the overlay.
            this.HighlightingBrush = new ScanditStyle.Brush(fillColor: Color.Transparent,
                                                            strokeColor: Color.White,
                                                      strokeWidth: 3);
            RectangularLocationSelection rectangularSelection = RectangularLocationSelection.Create(
                      new SizeWithUnit(
                        new FloatWithUnit(0.80f, MeasureUnit.Fraction),
                        new FloatWithUnit(0.32f, MeasureUnit.Fraction)));
            BarcodeCaptureSettings.LocationSelection = rectangularSelection;
            BarcodeCapture.ApplySettingsAsync(BarcodeCaptureSettings);
            //    MarginsWithUnit margins = new MarginsWithUnit(
            //              new FloatWithUnit(40.0f, MeasureUnit.Dip),
            //              new FloatWithUnit(0.0f, MeasureUnit.Dip),
            //              new FloatWithUnit(40.0f, MeasureUnit.Dip),
            //              new FloatWithUnit(0.5f, MeasureUnit.Fraction)
            //            );
            //dataCaptureView.ScanAreaMargins = margins;
        }
        public void BarCodeDispose()
        {
            if(this.BarcodeCapture != null)
            {
                this.BarcodeCapture.RemoveListener(this);
                this.BarcodeCapture = null;
            }
        }

        public async Task OnResumeAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                {
                    await this.ResumeFrameSource();
                }
            }
            else
            {
                await this.ResumeFrameSource();
            }
            this.OnPropertyChanged(nameof(ScanAreaMargins));
        }
        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }
        public Task OnSleep()
        {
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }
        #region IBarcodeCaptureListener
        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        { }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        { }

        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            //barcodeCapture.Enabled = false;

            MessagingCenter.Subscribe<string>(this, "EnableScan", async (sender) =>
            {
                barcodeCapture.Enabled = true;
                IsLoading = false;

            });
            MessagingCenter.Subscribe<string>(this, "DisableScan", (sender) =>
            {
                barcodeCapture.Enabled = false;

            });
            if (!session.NewlyRecognizedBarcodes.Any())
            {
                return;
            }
            Scandit.DataCapture.Barcode.Data.Unified.Barcode barcode = session.NewlyRecognizedBarcodes[0];
            this.BarCode = barcode.Data;

            //SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            //string result = "Scanned: " + barcode.Data + " (" + description.ReadableName + ")";
            Device.InvokeOnMainThreadAsync(async () =>
            {
                if (!this.BarCode.Contains("http"))
                {
                    barcodeCapture.Enabled = false;
                    GetScannedShipment(this.BarCode);
                    barcodeCapture.Enabled = false;
                }

                //Navigation.PushAsync(new ParcelInfo());
            });

            //if (!session.NewlyRecognizedBarcodes.Any())
            //{
            //    return;
            //}
            //Barcode barcode = session.NewlyRecognizedBarcodes[0];
            //barcodeCapture.Enabled = true;
            //this.BarCode = barcode.Data;

            ////this.BarCode.Add(barcode.Data);
            //SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            //string result = "Scanned: " + barcode.Data + " (" + description.ReadableName + ")";
            //Device.InvokeOnMainThreadAsync( async() =>
            //{
            //   await PopupNavigation.Instance.PushAsync(new ParcelInfo());
            //});

        }

        private async void GetScannedShipment(string barCode)
        {
            var username = Preferences.Get("Username", "default_value");
            try
            {
                Dictionary<string, string> scanStartProperties = new Dictionary<string, string>();
                scanStartProperties.Add("DateTime", DateTime.UtcNow.ToString());
                scanStartProperties.Add("Barcode", barCode);
                scanStartProperties.Add("Username", username);

                Analytics.TrackEvent("Warehouse Scanning is started", scanStartProperties);
                IsLoading = true;
                var shipment = await _service.ShipmentWarehouseScanned(barCode);
                await Task.Delay(500);
                if (shipment != null && shipment.hasError == false)
                {
                    var count = Preferences.Get("WarehouseScannedCount", "0");
                    var newCount = Convert.ToInt32(count);
                    newCount = newCount + 1;
                    Preferences.Set("WarehouseScannedCount", newCount.ToString());
                    WarehouseScannedCount = newCount.ToString();
                    if (shipment.result != null)
                    {
                        string lengthAlert = string.Empty;
                        if (shipment.result.declaredLengthCount > 0)
                        {
                            lengthAlert = "Length Alert";
                        }
                        string instructions = string.Empty;
                        if (!string.IsNullOrEmpty(shipment.result.deliveryInstruction))
                        {
                            instructions = shipment.result.deliveryInstruction;
                        }
                        string localPostalCode = string.Empty;
                        var isLocalPost = await isLocalPostalAlert(shipment.result.deliveryPostcode);
                        if (isLocalPost)
                        {
                            localPostalCode = $"Local Delivery Alert: {shipment.result.deliveryPostcode}";
                        }
                        string worldOptionCode = string.Empty;
                        var isWorldOptions = await isWorldOptionsAlert(shipment.result.deliveryPostcode);
                        if (isWorldOptions)
                        {
                            worldOptionCode = $"World Options Alert: {shipment.result.deliveryPostcode}";
                        }
                        string[] str = shipment.result.parcelNumber.Split(' ');
                        var number = str[0];
                        var consignmmentItem = shipment.result.consignmentItems.Where(x => x.itemNumber == number).FirstOrDefault();
                        Preferences.Set("ConsignmentKey", consignmmentItem.id.ToString());
                        if (shipment.isAlreadyScanned)
                        {

                            await PopupNavigation.Instance.PushAsync(new AlreadyScanned(lengthAlert, instructions, shipment.result, localPostalCode, worldOptionCode));
                            IsLoading = false;
                        }
                        else
                        {
                            if (shipment.result.declaredLengthCount > 0 || !string.IsNullOrEmpty(instructions) || !string.IsNullOrEmpty(localPostalCode))
                            {
                                IsLoading = false;
                                await PopupNavigation.Instance.PushAsync(new Instructions(lengthAlert, instructions, localPostalCode, worldOptionCode) { BindingContext = this });
                            }

                            //if (!string.IsNullOrEmpty(shipment.result.deliveryInstruction))
                            //{
                            //    IsLoading = false;
                            //    await PopupNavigation.Instance.PushAsync(new Instructions(shipment.result.deliveryInstruction, shipment.result.deliveryPostcode) { BindingContext = this });
                            //}
                        }
                    }
                }
                if (PopupNavigation.PopupStack.Count == 0)
                {
                    IsLoading = false;
                    MessagingCenter.Send<string>("1", "EnableScan");
                }
                //if (shipment != null && shipment.hasError == false)
                //{
                //    Preferences.Set("ConsignmentKey", shipment.result.ToString());
                //}
                //var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
                //var response = await _service.GetScanShipment(shipmentID);
                //if (response != null && response.hasError == false)
                //{
                //    var page = PopupNavigation.Instance.PopupStack.FirstOrDefault();
                //    if (page == null)
                //    {
                //        if (response.result != null)
                //        {
                //            var result = response.result.ToString();
                //            if (result == "True" || result == "true")
                //            {
                //                await PopupNavigation.Instance.PushAsync(new AlreadyScanned());
                //                IsLoading = false;

                //            }
                //            else
                //            {
                //                await LoadData(shipmentID);
                //                IsLoading = false;
                //                //PopupNavigation.Instance.PushAsync(new ParcelInfo(shipmentID));
                //            }
                //        }
                //        else
                //        {
                //            await (Application.Current as App).MainPage.DisplayAlert("Alert", "Consignment not found", "OK");
                //            MessagingCenter.Send<string>("1", "EnableScan");
                //            IsLoading = false;

                //        }
                //    }

                //}
                Dictionary<string, string> scanEndProperties = new Dictionary<string, string>();
                scanEndProperties.Add("DateTime", DateTime.UtcNow.ToString());
                scanEndProperties.Add("Barcode", barCode);
                scanEndProperties.Add("Username", username);
                Analytics.TrackEvent("Warehouse Scanning is ended", scanEndProperties);

            }
            catch (Exception ex)
            {
                Dictionary<string, string> scanStartProperties = new Dictionary<string, string>();
                scanStartProperties.Add("DateTime", DateTime.UtcNow.ToString());
                scanStartProperties.Add("Barcode", barCode);
                scanStartProperties.Add("Username", username);
                Crashes.TrackError(ex, scanStartProperties);
                MessagingCenter.Send<string>("1", "EnableScan");
                IsLoading = false;
            }
        }

        public async Task<bool> isLocalPostalAlert(string pC)
        {
            List<string> localPostalCodeList = new List<string>();
            localPostalCodeList.Add("NG1");
            localPostalCodeList.Add("NG2");
            localPostalCodeList.Add("NG3");
            localPostalCodeList.Add("NG5");
            localPostalCodeList.Add("NG6");
            localPostalCodeList.Add("NG7");
            localPostalCodeList.Add("NG14");
            localPostalCodeList.Add("NG15");
            localPostalCodeList.Add("NG16");
            localPostalCodeList.Add("NG17");
            localPostalCodeList.Add("NG18");
            localPostalCodeList.Add("NG19");
            localPostalCodeList.Add("S1");
            localPostalCodeList.Add("S2");
            localPostalCodeList.Add("S3");
            localPostalCodeList.Add("S4");
            localPostalCodeList.Add("S5");
            localPostalCodeList.Add("S6");
            localPostalCodeList.Add("S7");
            localPostalCodeList.Add("S8");
            localPostalCodeList.Add("S9");
            localPostalCodeList.Add("S10");
            localPostalCodeList.Add("S11");
            localPostalCodeList.Add("S40");
            localPostalCodeList.Add("S41");
            localPostalCodeList.Add("S42");
            localPostalCodeList.Add("S13");
            localPostalCodeList.Add("S60");
            localPostalCodeList.Add("S61");
            localPostalCodeList.Add("S65");
            foreach (var item in localPostalCodeList)
            {
                if (pC.Contains(" "))
                {
                    string[] str = pC.Split(' ');
                    if (str[0] == item)
                    {
                        return true;
                    }
                }
                else
                {
                    if (pC == item)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<bool> isWorldOptionsAlert(string pC)
        {
            List<string> localPostalCodeList = new List<string>();
            localPostalCodeList.Add("AB");
            localPostalCodeList.Add("BT");
            localPostalCodeList.Add("HS");
            localPostalCodeList.Add("IM");
            localPostalCodeList.Add("IV");
            localPostalCodeList.Add("KA");
            localPostalCodeList.Add("KW");
            localPostalCodeList.Add("PA");
            localPostalCodeList.Add("PH");
            localPostalCodeList.Add("P030");
            localPostalCodeList.Add("P031");
            localPostalCodeList.Add("PO32");
            localPostalCodeList.Add("PO33");
            localPostalCodeList.Add("PO34");
            localPostalCodeList.Add("PO35");
            localPostalCodeList.Add("PO36");
            localPostalCodeList.Add("PO37");
            localPostalCodeList.Add("PO38");
            localPostalCodeList.Add("PO39");
            localPostalCodeList.Add("PO40");
            localPostalCodeList.Add("PO41");
            localPostalCodeList.Add("TR21");
            localPostalCodeList.Add("TR22");
            localPostalCodeList.Add("TR23");
            localPostalCodeList.Add("TR24");
            localPostalCodeList.Add("TR25");
            foreach (var item in localPostalCodeList)
            {
                if (pC.Contains(" "))
                {
                    string[] str = pC.Split(' ');
                    if (str[0] == item)
                    {
                        return true;
                    }
                }
                else
                {
                    if (pC == item)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public async Task LoadData(string shipmentId)
        //{
        //    //this.IsRunning = true;
        //    var shipmentDetails = await _service.GetShipmentById(shipmentId);
        //    if (shipmentDetails != null)
        //    {

        //        if (shipmentDetails.result.declaredLengthCount > 0)
        //        {
        //            IsLoading = false;
        //            await PopupNavigation.Instance.PushAsync(new Instructions("Length Alert", shipmentDetails.result.deliveryPostcode) { BindingContext = this });
        //        }


        //        if (!string.IsNullOrEmpty(shipmentDetails.result.deliveryInstruction))
        //        {
        //            IsLoading = false;
        //            await PopupNavigation.Instance.PushAsync(new Instructions(shipmentDetails.result.deliveryInstruction, shipmentDetails.result.deliveryPostcode) { BindingContext = this });
        //        }

        //        var consignmmentItem = shipmentDetails.result.consignmentItems.Where(x => x.id == shipmentId).FirstOrDefault();
        //        //Length = consignmmentItem.palletLength.ToString();
        //        //ShipmentDetails = shipmentDetails.result;
        //        if (string.IsNullOrEmpty(shipmentDetails.result.notes))
        //        {
        //            //ShipmentDetails.deliveryInstruction = shipmentDetails.result.deliveryInstruction;
        //        }
        //        else
        //        {
        //            // ShipmentDetails.deliveryInstruction = shipmentDetails.result.notes;
        //        }
        //        await Utilty.SetSecureStorageValue(Utilty.HoldandReleaseKey, shipmentDetails.result.trackStatus);
        //        await SetScannedShipment();
        //    }
        //}
        //private async Task SetScannedShipment()
        //{
        //    try
        //    {
        //        var shipmentID = Preferences.Get("ConsignmentKey", "default_value");
        //        var obj = new SetShipmentModel()
        //        {
        //            ids = new List<string>()
        //            {
        //                shipmentID
        //            }
        //        };
        //        var response = await _service.SetScanShipment(obj);
        //        if (response != null && response.hasError == false)
        //        {
        //            if (PopupNavigation.PopupStack.Count == 0)
        //            {
        //                MessagingCenter.Send<string>("1", "EnableScan");
        //            }
        //            IsLoading = false;

        //        }
        //        IsLoading = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessagingCenter.Send<string>("1", "EnableScan");
        //        IsLoading = false;

        //    }
        //}


        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

