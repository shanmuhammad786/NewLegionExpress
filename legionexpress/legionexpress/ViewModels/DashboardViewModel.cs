using legionexpress.Services;
using legionexpress.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly NotificationApiService _notificationApiService = new NotificationApiService();
        private int _unacknowledgedCount;
        private bool _hasUnreadNotifications;

        public int UnacknowledgedCount
        {
            get => _unacknowledgedCount;
            set
            {
                _unacknowledgedCount = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(BadgeText));
                HasUnreadNotifications = value > 0;
            }
        }

        public bool HasUnreadNotifications
        {
            get => _hasUnreadNotifications;
            set
            {
                _hasUnreadNotifications = value;
                NotifyPropertyChanged();
            }
        }

        public string BadgeText => UnacknowledgedCount > 99 ? "99+" : UnacknowledgedCount.ToString();

        public DashboardViewModel()
        {
            MessagingCenter.Subscribe<object, int>(this, "NotificationBadgeRefresh", (s, count) =>
            {
                Device.BeginInvokeOnMainThread(() => UnacknowledgedCount = count);
            });
            MessagingCenter.Subscribe<object>(this, CollectionAlertPresenter.NotificationsUpdatedMessage, sender =>
            {
                Device.BeginInvokeOnMainThread(async () => await RefreshBadgeAsync());
            });
            AppNotificationManager.Instance.UnacknowledgedCountMayHaveChanged += OnSignalRMayHaveChangedBadge;
        }

        public async Task InitializeAsync()
        {
            if (Device.RuntimePlatform == Device.Android)
                await AppNotificationManager.Instance.StartAsync();

            await RefreshBadgeAsync();
        }

        public async Task RefreshBadgeAsync()
        {
            try
            {
                var response = await _notificationApiService.GetUserNotifications();
                if (response != null && !response.HasError && response.Result != null)
                    UnacknowledgedCount = response.Result.UnacknowledgedCount;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Badge refresh failed: {ex.Message}");
            }
        }

        private void OnSignalRMayHaveChangedBadge()
        {
            Device.BeginInvokeOnMainThread(async () => await RefreshBadgeAsync());
        }

        #region Commands
        public ICommand WarehouseCommand => new Command(WareHouse);
        public ICommand CollectionCommand => new Command(Collection);
        public ICommand ColDelListCommand => new Command(ColDelList);
        public ICommand DeliveryScanCommand => new Command(DeliveryScan);
        public ICommand NotificationsCommand => new Command(OpenNotifications);
        #endregion
        #region Methods
        private async void WareHouse()
        {
            await (Application.Current.MainPage).Navigation.PushAsync(new WarehouseHome());
        }
        private async void Collection()
        {
            await (Application.Current.MainPage).Navigation.PushAsync(new ScanditCollectionScanning());
        }
        private async void ColDelList()
        {
            await (Application.Current.MainPage).Navigation.PushAsync(new ColDelList());
        }
        private async void DeliveryScan()
        {
        }
        private async void OpenNotifications()
        {
            await (Application.Current.MainPage).Navigation.PushAsync(new NotificationsPage());
        }
        #endregion
    }
}
