using System;
using System.Collections.Generic;

namespace legionexpress.Models
{
	public class ShipmentCollectionDetailsModel
	{
		public ShipmentCollectionResult result { get; set; }
		public string errorMessage { get; set; }
		public bool hasError { get; set; }
	}
    public class ShipmentCollectionResult
    {
        public ShipmentResult shipment { get; set; }
        public bool isCollectionScanned { get; set; }
        public string id { get; set; }
        public string customerName { get; set; }
        public int itemToScanCount { get; set; }
        public int outstandingItemToScanCount { get; set; }
    }
   
}

