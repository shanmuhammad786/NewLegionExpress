using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace legionexpress.Models
{
    public class DriverCollectionResponse
    {
        public DriverCollectionResult Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class DriverCollectionResult
    {
        public List<DriverCollection> Collections { get; set; }
        public int RecordCount { get; set; }
    }

    public class DriverCollection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id { get; set; }
        public string Postcode { get; set; }
        public string Address1 { get; set; }
        public string CustomerName { get; set; }
        public string RunNumber { get; set; }
        public string Notes { get; set; }
        public string DeclineNotes { get; set; }
        public string Status { get; set; }
        public string CollectionTimeFrom { get; set; }
        public string CollectionTimeTo { get; set; }
        public bool IsViewedByDriver { get; set; }
        public string FullAddress => $"{Address1} - {Postcode}";
        public DateTime RunDate { get; set; }

        private bool _isInfoVisible;
        public bool IsInfoVisible
        {
            get => _isInfoVisible;
            set
            {
                if (_isInfoVisible != value)
                {
                    _isInfoVisible = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}

