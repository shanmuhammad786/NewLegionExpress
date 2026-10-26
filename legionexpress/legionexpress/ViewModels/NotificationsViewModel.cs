using legionexpress.Models;
using legionexpress.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        private readonly NotificationApiService _notificationApiService = new NotificationApiService();
        private readonly CollectionAlertPresenter _alertPresenter = new CollectionAlertPresenter();
        private ObservableCollection<NotificationListItem> _notifications =
            new ObservableCollection<NotificationListItem>();
        private bool _isLoading;
        private bool _isEmpty;

        public ObservableCollection<NotificationListItem> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand RefreshCommand => new Command(async () => await LoadAsync());

        public NotificationsViewModel()
        {
            MessagingCenter.Subscribe<object>(this, CollectionAlertPresenter.NotificationsUpdatedMessage, sender =>
            {
                Device.BeginInvokeOnMainThread(async () => await LoadAsync());
            });
        }

        public async Task LoadAsync()
        {
            try
            {
                IsLoading = true;
                var response = await _notificationApiService.GetUserNotifications();
                Notifications.Clear();

                var logs = response?.Result?.NotificationLogs;
                if (logs != null)
                {
                    foreach (var log in logs.Where(l => l != null && !l.IsAcknowledged))
                    {
                        Notifications.Add(NotificationListItem.FromLog(log));
                    }
                }

                IsEmpty = Notifications.Count == 0;
                MessagingCenter.Send<object, int>(
                    new object(),
                    "NotificationBadgeRefresh",
                    response?.Result?.UnacknowledgedCount ?? 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Load notifications failed: {ex.Message}");
                IsEmpty = Notifications.Count == 0;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task OpenNotificationAsync(NotificationListItem item)
        {
            if (item == null || item.Alerts == null || item.Alerts.Count == 0)
                return;

            await _alertPresenter.ShowAlertsAsync(
                item.Alerts,
                refreshListAsync: null,
                syncNotificationCenter: true);
        }
    }
}
