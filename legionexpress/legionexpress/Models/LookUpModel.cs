using legionexpress.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace legionexpress.Models
{
    public class LookUpModel
    {
        public List<Network> result { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }
    public class Network: BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Image { get; set; }
        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                NotifyPropertyChanged();
            }
        }
    }
}
