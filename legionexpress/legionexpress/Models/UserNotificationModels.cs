using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace legionexpress.Models
{
    public class UserNotificationsResponse
    {
        [JsonProperty("result")]
        public UserNotificationsResult Result { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }
    }

    public class UserNotificationsResult
    {
        [JsonProperty("unacknowledgedCount")]
        public int UnacknowledgedCount { get; set; }

        [JsonProperty("notificationLogs")]
        public List<NotificationLogModel> NotificationLogs { get; set; }
    }

    public class NotificationLogModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("isAcknowledged")]
        public bool IsAcknowledged { get; set; }

        public NotificationPayloadModel ParsedPayload
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Payload))
                    return null;

                try
                {
                    return JsonConvert.DeserializeObject<NotificationPayloadModel>(Payload);
                }
                catch
                {
                    return null;
                }
            }
        }
    }

    public class AcknowledgeNotificationRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public class AcknowledgeNotificationResponse
    {
        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }
    }

    /// <summary>
    /// Display model for the notifications list page.
    /// </summary>
    public class NotificationListItem
    {
        public int NotificationLogId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string DetailLine { get; set; }
        public bool IsAcknowledged { get; set; }
        public NotificationPayloadModel ParsedPayload { get; set; }
        public List<CollectionAlertItem> Alerts { get; set; }

        public string TypeLabel =>
            string.Equals(Type, "NewCollectionNotes", StringComparison.OrdinalIgnoreCase)
                ? "Notes"
                : "Collection";

        public static NotificationListItem FromLog(NotificationLogModel log)
        {
            var payload = log?.ParsedPayload;
            var alerts = NotificationAlertFactory.BuildAlerts(payload, log?.Id);
            var first = alerts?.FirstOrDefault();
            var collectionCount = alerts?.Count(a => !a.IsNotesAlert) ?? 0;

            string summary;
            string detail;
            string title;

            if (string.Equals(log?.Type, "NewCollectionNotes", StringComparison.OrdinalIgnoreCase) ||
                (first != null && first.IsNotesAlert))
            {
                title = "New Collection Notes";
                summary = first?.CustomerName ?? "Collection notes";
                detail = string.IsNullOrWhiteSpace(first?.Notes)
                    ? (first?.Postcode ?? string.Empty)
                    : $"{first.Postcode} · {first.Notes}";
            }
            else if (collectionCount > 1)
            {
                title = "Collection Assigned";
                summary = $"{collectionCount} collections assigned";
                detail = first?.CustomerName ?? string.Empty;
            }
            else
            {
                title = "Collection Assigned";
                summary = first?.CustomerName ?? "Collection assigned";
                detail = first?.Postcode ?? string.Empty;
            }

            return new NotificationListItem
            {
                NotificationLogId = log?.Id ?? 0,
                Type = log?.Type,
                Title = title,
                Summary = summary,
                DetailLine = detail,
                IsAcknowledged = log?.IsAcknowledged ?? false,
                ParsedPayload = payload,
                Alerts = alerts ?? new List<CollectionAlertItem>()
            };
        }
    }

    public static class NotificationAlertFactory
    {
        public static List<CollectionAlertItem> BuildAlerts(NotificationPayloadModel notification, int? notificationLogId = null)
        {
            if (notification?.Data == null || string.IsNullOrWhiteSpace(notification.Event))
                return new List<CollectionAlertItem>();

            if (string.Equals(notification.Event, "CollectionAssigned", StringComparison.OrdinalIgnoreCase))
                return BuildCollectionAssignedAlerts(notification.Data, notificationLogId);

            if (string.Equals(notification.Event, "NewCollectionNotes", StringComparison.OrdinalIgnoreCase))
                return BuildNewCollectionNotesAlerts(notification.Data, notificationLogId);

            return new List<CollectionAlertItem>();
        }

        public static List<CollectionAlertItem> BuildCollectionAssignedAlerts(NotificationDataModel data, int? notificationLogId = null)
        {
            var alerts = new List<CollectionAlertItem>();
            if (data == null)
                return alerts;

            if (data.Collections != null && data.Collections.Count > 0)
            {
                foreach (var collection in data.Collections)
                {
                    if (collection == null || collection.Id <= 0)
                        continue;

                    alerts.Add(new CollectionAlertItem
                    {
                        Id = collection.Id,
                        CustomerName = collection.CustomerName,
                        Postcode = collection.PostCode,
                        AlertType = CollectionAlertType.CollectionAssigned,
                        NotificationLogId = notificationLogId
                    });
                }

                return alerts;
            }

            if (data.Id > 0)
            {
                alerts.Add(new CollectionAlertItem
                {
                    Id = data.Id,
                    CustomerName = data.CustomerName,
                    Postcode = data.PostCode,
                    AlertType = CollectionAlertType.CollectionAssigned,
                    NotificationLogId = notificationLogId
                });
            }

            return alerts;
        }

        public static List<CollectionAlertItem> BuildNewCollectionNotesAlerts(NotificationDataModel data, int? notificationLogId = null)
        {
            var alerts = new List<CollectionAlertItem>();
            if (data == null)
                return alerts;

            var collection = data.Collection;
            if (collection != null && collection.Id > 0)
            {
                alerts.Add(new CollectionAlertItem
                {
                    Id = collection.Id,
                    CustomerName = collection.CustomerName,
                    Postcode = collection.PostCode,
                    Notes = collection.Notes ?? data.Notes,
                    AlertType = CollectionAlertType.NewCollectionNotes,
                    NotificationLogId = notificationLogId
                });
                return alerts;
            }

            if (data.Id > 0)
            {
                alerts.Add(new CollectionAlertItem
                {
                    Id = data.Id,
                    CustomerName = data.CustomerName,
                    Postcode = data.PostCode,
                    Notes = data.Notes,
                    AlertType = CollectionAlertType.NewCollectionNotes,
                    NotificationLogId = notificationLogId
                });
            }

            return alerts;
        }
    }

    /// <summary>
    /// Handles payload.data as either an object or a stringified JSON object.
    /// </summary>
    public class NotificationDataJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(NotificationDataModel);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                var raw = reader.Value?.ToString();
                if (string.IsNullOrWhiteSpace(raw))
                    return null;
                return JsonConvert.DeserializeObject<NotificationDataModel>(raw);
            }

            var obj = JObject.Load(reader);
            return obj.ToObject<NotificationDataModel>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
