using System;
using System.Collections.Generic;
using System.Text;

namespace legionexpress.Models
{
    public class ScanResponseModel
    {
        public object result { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }
}
