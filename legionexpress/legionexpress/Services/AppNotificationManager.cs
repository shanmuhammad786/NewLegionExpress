using legionexpress.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace legionexpress.Services
{
    /// <summary>
    /// App-wide Android SignalR connection. Live alerts are delivered only while ColDelList
    /// is active; pending alerts when opening ColDelList come from the notification API.
    /// </summary>
    public class AppNotificationManager
    {
        private static readonly Lazy<AppNotificationManager> _instance =
            new Lazy<AppNotificationManager>(() => new AppNotificationManager());

        public static AppNotificationManager Instance => _instance.Value;

        private readonly NotificationHubService _hubService = new NotificationHubService();

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
                        LiveAlertsReceived?.Invoke(alerts);

                    UnacknowledgedCountMayHaveChanged?.Invoke();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"AppNotificationManager handle failed: {ex.Message}");
                }
            });
        }
    }
}
