using legionexpress.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace legionexpress.Services
{
    internal class ShipmentService : MobileServiceBase
    {
        public async Task<ScanResponseModel> GetShipmentByNetworkId(string networkId)
        {
            string url = $"api/v1/shipment/get-shipment-id-by-network-shipment-id?NetworkShipmentId={networkId}";

            return await GetAuthorized<ScanResponseModel>(url);
        }

        public async Task<ShipmentDetailsModel> GetShipmentById(string networkId)
        {
            string url = $"api/v1/shipment/get-shipment-by-id?Id={networkId}";

            return await GetAuthorized<ShipmentDetailsModel>(url);
        }

        public async Task<ScanResponseModel> HoldShipment(string notes,string shipmentId)
        {
            var shipObj = new ShipmentId();
            shipObj.Id = shipmentId;
            shipObj.Notes = notes;
            string url = $"api/v1/shipment/request-hold-shipment";

           return await HoldShipment(shipObj, url);
        }

        public async Task<ScanResponseModel>AddNote(Object obj)
        {
            string url = $"api/v1/shipment/update-shipment-notes";
            return await AddNote(obj, url);
        }
        public async Task<ScanResponseModel> ReleaseShipment(string notes,string shipmentId)
        {
            var shipObj = new ShipmentId();
            shipObj.Id = shipmentId;
            shipObj.Notes = notes;
            string url = $"api/v1/shipment/request-release-shipment";

           return await ReleaseShipment(shipObj, url);
        }

        public async Task<ScanResponseModel> LabelReprint(string shipmentId)
        {
            var shipObj = new ShipmentId();
            shipObj.Id = shipmentId;
            string url = $"api/v1/shipment/request-reprint-label";

            return await LabelReprint(shipObj, url);
        }

        public async Task UpdateNetwork(string shipmentId, int networkId)
        {
            var networkObj = new NetworkUpdateDto();
            networkObj.Id = shipmentId;
            networkObj.NetworkId = networkId;

            string url = $"/api/v1/shipment/request-reprint-label";

            await PostAuthorized<NetworkUpdateDto>(networkObj, url);
        }

        public async Task<ScanResponseModel> UpdateDimensions(DimensionUpdateDto dimensionUpdate)
        {

            string url = $"api/v1/shipment/request-update-shipment-dimensions";

           return await UpdateDimenstions(dimensionUpdate, url);
        }
        public async Task<ScanResponseModel> SetScanShipment(object obj)
        {
            string url = $"api/v1/shipment/set-shipment-collection-scanned";
            return await SetScannedShipment(obj,url);
        }
        public async Task<ScanResponseModel> GetScanShipment(string shipmentID)
        {
            string url = $"api/v1/shipment/is-shipment-collection-scanned?Id={shipmentID}";
            return await GetScannedShipment(url);
        }

        public async Task<ShipmentDetailsModel> ShipmentCollectionScanned(string networkId)
        {
            var obj = new
            {
                networkShipmentId = networkId
            };
            string url = $"api/v1/shipment/shipment-collection-scan";
            return await PostShipmentCollectionScanned<ShipmentDetailsModel>(obj,url);
        }
    }
}

public class ShipmentId
{
    public string Id { get; set; }
    public string Notes { get; set; }
}

public class NetworkUpdateDto
{
    public string Id { get; set; }
    public int NetworkId { get; set; }
}

public class DimensionUpdateDto
{
    public string Id { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
}