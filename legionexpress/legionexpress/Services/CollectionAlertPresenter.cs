using legionexpress.Models;
using legionexpress.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace legionexpress.Services
{
    /// <summary>
    /// Shared accept / decline / notes popup flow used by ColDelList and Notifications list.
    /// </summary>
    public class CollectionAlertPresenter
    {
        public const string NotificationsUpdatedMessage = "NotificationsUpdated";

        private readonly ShipmentService _shipmentService = new ShipmentService();
        private readonly NotificationApiService _notificationApiService = new NotificationApiService();
        private NewCollectionRequestPopup _activePopup;

        public bool IsShowing => _activePopup != null;

        public async Task ShowAlertsAsync(
            IList<CollectionAlertItem> alerts,
            Func<Task> refreshListAsync = null,
            bool syncNotificationCenter = true)
        {
            if (alerts == null || alerts.Count == 0)
                return;

            var newAlerts = alerts
                .Where(a => a != null && a.Id > 0)
                .Where(a => _activePopup == null || !_activePopup.ContainsAlert(a))
                .GroupBy(a => new { a.Id, a.AlertType })
                .Select(g => g.First())
                .ToList();

            if (newAlerts.Count == 0)
                return;

            if (_activePopup != null)
            {
                foreach (var alert in newAlerts)
                    _activePopup.Enqueue(alert);
                return;
            }

            var notificationLogId = newAlerts
                .Select(a => a.NotificationLogId)
                .FirstOrDefault(id => id.HasValue && id.Value > 0);

            var isNotesStack = newAlerts.Any(a => a.IsNotesAlert);

            _activePopup = new NewCollectionRequestPopup(
                newAlerts[0],
                alert => AcceptAsync(alert, refreshListAsync),
                alert => DeclineAsync(alert, refreshListAsync),
                alert => AcknowledgeNotesAsync(alert, refreshListAsync, syncNotificationCenter),
                onClosed: () => _activePopup = null,
                onStackCompleted: async () =>
                {
                    if (!syncNotificationCenter)
                        return;

                    // Notes stacks already acknowledge on OK inside AcknowledgeNotesAsync.
                    if (isNotesStack)
                    {
                        NotifyNotificationsUpdated();
                        return;
                    }

                    // Collection stacks: acknowledge every distinct notification log in the stack.
                    var logIds = newAlerts
                        .Where(a => a != null && !a.IsNotesAlert)
                        .Select(a => a.NotificationLogId)
                        .Where(id => id.HasValue && id.Value > 0)
                        .Select(id => id.Value)
                        .Distinct()
                        .ToList();

                    if (logIds.Count == 0)
                    {
                        var logId = notificationLogId;
                        if (!logId.HasValue || logId.Value <= 0)
                            logId = await FindMatchingNotificationLogIdAsync(newAlerts);

                        if (logId.HasValue && logId.Value > 0)
                            logIds.Add(logId.Value);
                    }

                    if (logIds.Count > 0)
                        await AcknowledgeNotificationLogsAsync(logIds);
                    else
                        NotifyNotificationsUpdated();
                });

            for (var i = 1; i < newAlerts.Count; i++)
                _activePopup.Enqueue(newAlerts[i]);

            await PopupNavigation.Instance.PushAsync(_activePopup);
        }

        public async Task DismissActivePopupAsync()
        {
            if (_activePopup == null)
                return;

            try
            {
                if (PopupNavigation.Instance.PopupStack.Contains(_activePopup))
                    await PopupNavigation.Instance.RemovePageAsync(_activePopup);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Dismiss alert popup failed: {ex.Message}");
            }
            finally
            {
                _activePopup = null;
            }
        }

        private async Task<bool> AcceptAsync(CollectionAlertItem alert, Func<Task> refreshListAsync)
        {
            try
            {
                var request = new AcceptCollectionRequestListModel
                {
                    ids = new List<int> { alert.Id }
                };
                var response = await _shipmentService.AcceptCollections(request);
                if (response != null && !response.HasError)
                {
                    if (refreshListAsync != null)
                        await refreshListAsync();
                    return true;
                }

                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", response?.ErrorMessage ?? "Unable to accept collection"));
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Accept alert failed: {ex.Message}");
                await PopupNavigation.Instance.PushAsync(new AlertPopup("Error", "Unable to accept collection"));
                return false;
            }
        }

        private async Task<bool> DeclineAsync(CollectionAlertItem alert, Func<Task> refreshListAsync)
        {
            try
            {
                var request = new CompleteCollectionRequestModel { id = alert.Id };
                var response = await _shipmentService.DeclineCollections(request);
                if (response != null && !response.HasError)
                {
                    if (refreshListAsync != null)
                        await refreshListAsync();
                    return true;
                }

                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", response?.ErrorMessage ?? "Unable to decline collection"));
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Decline alert failed: {ex.Message}");
                await PopupNavigation.Instance.PushAsync(new AlertPopup("Error", "Unable to decline collection"));
                return false;
            }
        }

        private async Task<bool> AcknowledgeNotesAsync(
            CollectionAlertItem alert,
            Func<Task> refreshListAsync,
            bool syncNotificationCenter)
        {
            try
            {
                if (refreshListAsync != null)
                    await refreshListAsync();

                if (syncNotificationCenter)
                {
                    var logId = alert.NotificationLogId;
                    if (!logId.HasValue || logId.Value <= 0)
                        logId = await FindMatchingNotificationLogIdAsync(new List<CollectionAlertItem> { alert });

                    if (logId.HasValue && logId.Value > 0)
                        await AcknowledgeNotificationLogsAsync(new[] { logId.Value });
                    else
                        NotifyNotificationsUpdated();
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notes alert failed: {ex.Message}");
                if (syncNotificationCenter)
                    NotifyNotificationsUpdated();
                return true;
            }
        }

        private async Task<int?> FindMatchingNotificationLogIdAsync(IList<CollectionAlertItem> alerts)
        {
            try
            {
                if (alerts == null || alerts.Count == 0)
                    return null;

                var response = await _notificationApiService.GetUserNotifications();
                var logs = response?.Result?.NotificationLogs;
                if (logs == null || logs.Count == 0)
                    return null;

                var alertIds = alerts.Select(a => a.Id).OrderBy(x => x).ToList();
                var isNotes = alerts.Any(a => a.IsNotesAlert);
                var expectedType = isNotes ? "NewCollectionNotes" : "CollectionAssigned";

                foreach (var log in logs.Where(l => l != null && !l.IsAcknowledged))
                {
                    if (!string.Equals(log.Type, expectedType, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var listItem = NotificationListItem.FromLog(log);
                    var logAlertIds = listItem.Alerts?
                        .Select(a => a.Id)
                        .OrderBy(x => x)
                        .ToList() ?? new List<int>();

                    if (logAlertIds.Count == 0)
                        continue;

                    // Exact set match, or SignalR single/partial subset of a multi-collection notification.
                    if (alertIds.SequenceEqual(logAlertIds) ||
                        alertIds.All(id => logAlertIds.Contains(id)))
                    {
                        return log.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Find matching notification failed: {ex.Message}");
            }

            return null;
        }

        private async Task AcknowledgeNotificationLogsAsync(IEnumerable<int> notificationLogIds)
        {
            if (notificationLogIds == null)
            {
                NotifyNotificationsUpdated();
                return;
            }

            foreach (var notificationLogId in notificationLogIds.Distinct())
            {
                if (notificationLogId <= 0)
                    continue;

                try
                {
                    await _notificationApiService.SetNotificationAcknowledged(notificationLogId);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Acknowledge notification log failed: {ex.Message}");
                }
            }

            NotifyNotificationsUpdated();
        }

        public static void NotifyNotificationsUpdated()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send<object>(new object(), NotificationsUpdatedMessage);
            });
        }
    }
}
