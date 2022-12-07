using System.ComponentModel;
using System.Runtime.CompilerServices;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using System.Linq;
using Scandit.DataCapture.Barcode.Data.Unified;
using legionexpress.Models;
using ScanditStyle = Scandit.DataCapture.Core.UI.Style.Unified;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using legionexpress.Views;

namespace legionexpress.ViewModels
{
    class ScanditExceptionScanViewModel : INotifyPropertyChanged, IBarcodeCaptureListener
    {
        private IViewfinder viewfinder;
        private ScanditStyle.Brush highlightingBrush;
        public Camera Camera { get; private set; } = ScannerModel.Instance.CurrentCamera;

        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; } = ScannerModel.Instance.BarcodeCaptureSettings;
        public DataCaptureContext DataCaptureContext { get; private set; } = ScannerModel.Instance.DataCaptureContext;


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
        public INavigation Navigation { get; set; }

        public ScanditExceptionScanViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.InitializeScanner();
        }
        private void InitializeScanner()
        {
            this.BarcodeCapture.AddListener(this);
            this.Viewfinder = new RectangularViewfinder(RectangularViewfinderStyle.Square, RectangularViewfinderLineStyle.Light);
            this.HighlightingBrush = new ScanditStyle.Brush(fillColor: Color.Transparent,
                                                            strokeColor: Color.White,
                                                            strokeWidth: 3);
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
        }
        private Task ResumeFrameSource()
        {
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }
        public Task OnSleep()
        {
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }
        #region IBarcodeCaptureListener
        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        { 
        }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        {
        }

        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            if (!session.NewlyRecognizedBarcodes.Any())
            {
                return;
            }
            Barcode barcode = session.NewlyRecognizedBarcodes[0];
            barcodeCapture.Enabled = true;
            this.BarCode = barcode.Data;
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string result = "Scanned: " + barcode.Data + " (" + description.ReadableName + ")";
            Device.InvokeOnMainThreadAsync( () =>
            {
                 Navigation.PushAsync(new ExemptionScreen(this.BarCode));
            });
            //MessagingCenter.Send(this.BarCode, "AddNew");
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
