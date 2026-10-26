using legionexpress.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace legionexpress.Services
{
    /// <summary>
    /// App-wide Android SignalR connection + offline queue for Col/Del alerts.
    /// </summary>
    public class AppNotificationManager
    {
        private static readonly Lazy<AppNotificationManager> _instance =
            new Lazy<AppNotificationManager>(() => new AppNotificationManager());

        public static AppNotificationManager Instance => _instance.Value;

        private readonly NotificationHubService _hubService = new NotificationHubService();
        private readonly object _queueLock = new object();
        private readonly List<CollectionAlertItem> _queuedAlerts = new List<CollectionAlertItem>();

        public bool IsColDelListActive { get; private set; }

        /// <summary>
        /// Raised on the main thread when live alerts should be shown immediately on ColDelList.
        /// </summary>
        public event Action<IList<CollectionAlertItem>> LiveAlertsReceived;

        public event Action UnacknowledgedCountMayHaveChanged;

        private AppNotificationManager()
        {
        }

        public async Task StartAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            _hubService.MessageReceived -= OnMessageReceived;
            _hubService.MessageReceived += OnMessageReceived;
            await _hubService.ConnectAsync();
        }

        public async Task StopAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            _hubService.MessageReceived -= OnMessageReceived;
            ClearQueue();
            IsColDelListActive = false;
            await _hubService.DisconnectAsync();
        }

        public void EnterColDelList()
        {
            IsColDelListActive = true;
        }

        public void LeaveColDelList()
        {
            IsColDelListActive = false;
        }

        public List<CollectionAlertItem> TakeQueuedAlerts()
        {
            lock (_queueLock)
            {
                var items = _queuedAlerts.ToList();
                _queuedAlerts.Clear();
                return items;
            }
        }

        public void RemoveAlertsFromQueue(IEnumerable<CollectionAlertItem> alerts)
        {
            if (alerts == null)
                return;

            lock (_queueLock)
            {
                foreach (var alert in alerts)
                {
                    if (alert == null)
                        continue;

                    _queuedAlerts.RemoveAll(x =>
                        x.Id == alert.Id && x.AlertType == alert.AlertType);
                }
            }
        }

        public void ClearQueue()
        {
            lock (_queueLock)
            {
                _queuedAlerts.Clear();
            }
        }

        private void OnMessageReceived(string payload)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(payload))
                        return;

                    var notification = JsonConvert.DeserializeObject<NotificationPayloadModel>(payload);
                    var alerts = NotificationAlertFactory.BuildAlerts(notification);
                    if (alerts == null || alerts.Count == 0)
                        return;

                    if (IsColDelListActive)
                    {
                        LiveAlertsReceived?.Invoke(alerts);
                    }
                    else
                    {
                        EnqueueAlerts(alerts);
                    }

                    UnacknowledgedCountMayHaveChanged?.Invoke();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"AppNotificationManager handle failed: {ex.Message}");
                }
            });
        }

        private void EnqueueAlerts(IList<CollectionAlertItem> alerts)
        {
            lock (_queueLock)
            {
                foreach (var alert in alerts)
                {
                    if (alert == null || alert.Id <= 0)
                        continue;

                    if (_queuedAlerts.Any(x => x.Id == alert.Id && x.AlertType == alert.AlertType))
                        continue;

                    _queuedAlerts.Add(alert);
                }
            }
        }
    }
}
