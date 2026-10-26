using System;

namespace legionexpress.Models
{
    public class ShipmentWarehouseDetailsModel
    {
        public ShipmentWarehouseResult result { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }

    public class ShipmentWarehouseResult
    {
        public ShipmentResult shipment { get; set; }
        public bool isWarehouseScanned { get; set; }
        public string id { get; set; }
    }
}
