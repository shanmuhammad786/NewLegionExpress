using Newtonsoft.Json;
using System.Collections.Generic;

namespace legionexpress.Models
{
    public enum CollectionAlertType
    {
        CollectionAssigned,
        NewCollectionNotes
    }

    public class NotificationPayloadModel
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public NotificationDataModel Data { get; set; }
    }

    public class NotificationDataModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("collections")]
        public List<NotificationCollectionModel> Collections { get; set; }

        [JsonProperty("collection")]
        public NotificationCollectionModel Collection { get; set; }
    }

    public class NotificationCollectionModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }

    public class CollectionAlertItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Postcode { get; set; }
        public string Notes { get; set; }
        public CollectionAlertType AlertType { get; set; }

        public bool IsNotesAlert => AlertType == CollectionAlertType.NewCollectionNotes;
    }
}
