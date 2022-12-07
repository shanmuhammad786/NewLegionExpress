using legionexpress.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class ReviewScanViewModel : INotifyPropertyChanged
    {
        private ScanModel _scanModel;
        public ScanModel ScanModel
        {
            get
            {
                return _scanModel;
            }
            set
            {
                _scanModel = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<ScanModel> _scanList;
        public ObservableCollection<ScanModel> ScanList
        {
            get
            {
                return _scanList;
            }
            set
            {
                _scanList = value;
                NotifyPropertyChanged();
            }
        }
        public ReviewScanViewModel()
        {
            this.GetScanList();
        }
        public async Task<ObservableCollection<ScanModel>> GetScanList()
        {
            ScanList = new ObservableCollection<ScanModel>();
            ScanList.Add(new ScanModel()
            {
                Id = 1,
                Number =12345,
                Company = "Ali Express",
                Postcode = "LE12B8y"
            });
            ScanList.Add(new ScanModel()
            {
                Id = 1,
                Number = 68926,
                Company = "Uber",
                Postcode = "LEC1748"
            });
            ScanList.Add(new ScanModel()
            {
                Id = 1,
                Number = 67863,
                Company = "Daraz",
                Postcode = "GAK2845"
            });
            ScanList.Add(new ScanModel()
            {
                Id = 1,
                Number = 89465,
                Company = "Amazon",
                Postcode = "SKT1128"
            });
            return ScanList;
        }
        public ICommand AddReviewCommand => new Command(async () => await AddReviewCommandAsync());
        private async Task AddReviewCommandAsync()
        {
        }
        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
