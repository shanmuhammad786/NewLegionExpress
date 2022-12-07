using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace legionexpress.Models
{
    public class ShipmentDetailsModel
    {
        public bool isAlreadyScanned { get; set; }
        public ShipmentResult result { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }
    public class ConsignmentTracking
    {
        public string location { get; set; }
        public string status { get; set; }
        public string sourceDescription { get; set; }
        public string description { get; set; }
        public bool hasAlert { get; set; }
        public bool isManualEntry { get; set; }
        public string podImage { get; set; }
        public bool hasPOD { get; set; }
        public string signedBy { get; set; }
        public bool isSuppress { get; set; }
        public string network { get; set; }
        public string networkConsignmentId { get; set; }
        public DateTime dateInserted { get; set; }
        public string colour { get; set; }
        public string colourLessOpacity { get; set; }
        public string note { get; set; }
        public DateTime trackingDate { get; set; }
        public string trackingDay { get; set; }
        public string trackingTime { get; set; }
        public string trackingDateFormatted { get; set; }
    }

    public class ConsignmentItem
    {
        public string id { get; set; }
        public string itemNumber { get; set; }
        public double? length { get; set; }
        public double? width { get; set; }
        public double? height { get; set; }
        public object palletLength { get; set; }
        public object palletWidth { get; set; }
        public object palletHeight { get; set; }
        public double? palletWeight { get; set; }
        public object palletSpace { get; set; }
        public string dimensionsChangeRequest { get; set; }
        public Color scanItemBackground { get; set; }
        public bool isCollectionScanned { get; set; }
        public double? changeRequestLength { get; set; }
        public double? changeRequestWidth { get; set; }
        public double? changeRequestHeight { get; set; }
        public double? changeRequestWeight { get; set; }
    }

    public class ShipmentResult
    {
        public string guidId { get; set; }
        public DateTime despatchDate { get; set; }
        public int consignmentStatusId { get; set; }
        public string noteNumber { get; set; }
        public int networkId { get; set; }
        public object networkConsignmentId { get; set; }
        public string deliveryName { get; set; }
        public string deliveryAddress1 { get; set; }
        public string deliveryAddress2 { get; set; }
        public string deliveryAddress3 { get; set; }
        public string deliveryAddress4 { get; set; }
        public string deliveryPostcode { get; set; }
        public string deliveryCountryCode { get; set; }
        public string deliveryCountry { get; set; }
        public string deliveryContact { get; set; }
        public string deliveryEmail { get; set; }
        public string deliveryTelephone { get; set; }
        public string deliveryReference { get; set; }
        public string deliveryInstruction { get; set; }
        public string collectionName { get; set; }
        public string collectionAddress1 { get; set; }
        public string collectionAddress2 { get; set; }
        public string collectionAddress3 { get; set; }
        public string collectionAddress4 { get; set; }
        public string collectionPostcode { get; set; }
        public string collectionCountryCode { get; set; }
        public string collectionCountry { get; set; }
        public string collectionContact { get; set; }
        public string collectionEmail { get; set; }
        public string collectionTelephone { get; set; }
        public string collectionReference { get; set; }
        public string collectionInstruction { get; set; }
        public bool isSendEmail { get; set; }
        public bool isSendSMS { get; set; }
        public bool isParcel { get; set; }
        public int numberOfItem { get; set; }
        public double weight { get; set; }
        public int serviceLevelId { get; set; }
        public double chargeableWeight { get; set; }
        public bool isBookingRequired { get; set; }
        public bool isBookingOpenTime { get; set; }
        public string bookingReference { get; set; }
        public object bookingDate { get; set; }
        public string bookingTime { get; set; }
        public string customerAccountCode { get; set; }
        public string customerName { get; set; }
        public string serviceLevelName { get; set; }
        public int manifestNumber { get; set; }
        public string trackSignedBy { get; set; }
        public object trackDeliveryDay { get; set; }
        public object trackDeliveryDate { get; set; }
        public object trackDeliveryTime { get; set; }
        public string trackStatus { get; set; }
        public string dimensionsChangeRequest { get; set; }
        public string networkChangeRequest { get; set; }
        public string statusChangeRequest { get; set; }
        public bool? labelRePrintRequest { get; set; }
        public bool? isChangeRequested { get; set; }
        public string notes { get; set; }
        public string parcelNumber { get; set; }
        public int declaredLengthCount { get; set; }
        public List<ConsignmentTracking> consignmentTrackings { get; set; }
        public List<ConsignmentItem> consignmentItems { get; set; }
    }
}
