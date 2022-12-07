//using Scandit.BarcodePicker.Unified;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class CollectionScanningViewModel : INotifyPropertyChanged
    {
        private string _recognizedCode;

        public string RecognizedCode
        {
            get
            {
                return (_recognizedCode == null) ? "" : "Code scanned: " + _recognizedCode;
            }

            set
            {
                _recognizedCode = value;
                OnPropertyChanged(nameof(RecognizedCode));
            }
        }
        public CollectionScanningViewModel()
        {
            //ScanditService.BarcodePicker.DidScan += BarcodePickerOnDidScan;
        }
        //public ICommand StartScanningCommand => new Command(async () => await StartScanning());
        //private async void BarcodePickerOnDidScan(ScanSession session)
        //{
        //    RecognizedCode = session.NewlyRecognizedCodes.LastOrDefault()?.Data;
        //    //await ScanditService.BarcodePicker.StopScanningAsync();
        //}
        //private async Task StartScanning()
        //{
        //    await ScanditService.BarcodePicker.StartScanningAsync(false);
        //    //ScanditService.BarcodePicker.ScanOverlay.si
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
