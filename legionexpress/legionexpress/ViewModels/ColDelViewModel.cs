using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
    public class ColDelViewModel:BaseViewModel
    {
        #region PrivateProperties
        private ObservableCollection<DriverCollection> _colDelList;
        private readonly ShipmentService _shipmentService;
        private readonly CollectionAlertPresenter _alertPresenter;
        private int _selectedFilter = 3;
        private bool _isLoading;
        #endregion
        #region PublicProperties
        public ObservableCollection<DriverCollection> ColDelList
        {
            get => _colDelList;
            set
            {
                _colDelList = value;
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
        public int SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AllButtonStyle));
                NotifyPropertyChanged(nameof(DoneButtonStyle));
            }
        }

        #region StylesFilter
        public Style AllButtonStyle => SelectedFilter == 3 ? SelectedButtonStyle : UnselectedButtonStyle;
        public Style DoneButtonStyle => SelectedFilter == 0 ? SelectedButtonStyle : UnselectedButtonStyle;

        private Style _selectedButtonStyle;
        public Style SelectedButtonStyle => _selectedButtonStyle ?? (_selectedButtonStyle = new Style(typeof(Button))
        {
            Setters = {
        new Setter { Property = Button.BackgroundColorProperty, Value = "#2E2E2E" },
        new Setter { Property = Button.TextColorProperty, Value = Color.White }
    }
        });

        private Style _unselectedButtonStyle;
        public Style UnselectedButtonStyle => _unselectedButtonStyle ?? (_unselectedButtonStyle = new Style(typeof(Button))
        {
            Setters = {
        new Setter { Property = Button.BackgroundColorProperty, Value = "#F2F2F2" },
        new Setter { Property = Button.TextColorProperty, Value = Color.Black },
        new Setter { Property = Button.BorderColorProperty, Value = "#5A5A5A" },
        new Setter { Property = Button.BorderWidthProperty, Value = 1 }
    }
        });
        #endregion
        #endregion

        public ColDelViewModel()
        {
            _shipmentService = new ShipmentService();
            _alertPresenter = new CollectionAlertPresenter();
            ColDelList = new ObservableCollection<DriverCollection>();
            MessagingCenter.Subscribe<object, bool>(this, "RefreshList", HandleRefresh);
            LoadList();
        }
        private void HandleRefresh(object sender, bool shouldRefresh)
        {
            if (shouldRefresh)
            {
                LoadList();
            }
        }

        public async Task OnPageAppearingAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            var manager = AppNotificationManager.Instance;
            manager.EnterColDelList();
            manager.LiveAlertsReceived -= OnLiveAlertsReceived;
            manager.LiveAlertsReceived += OnLiveAlertsReceived;

            await manager.StartAsync();

            var queued = manager.TakeQueuedAlerts();
            if (queued.Count > 0)
            {
                // Skip alerts already handled via notification list / acknowledge API.
                queued = await FilterStillUnacknowledgedAlertsAsync(queued);
            }

            if (queued.Count > 0)
            {
                await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                await _alertPresenter.ShowAlertsAsync(
                    queued,
                    () => LoadList(SelectedFilter == 0 ? (int?)0 : null),
                    syncNotificationCenter: true);
            }
        }

        private async Task<List<CollectionAlertItem>> FilterStillUnacknowledgedAlertsAsync(List<CollectionAlertItem> queued)
        {
            try
            {
                var api = new NotificationApiService();
                var response = await api.GetUserNotifications();
                var logs = response?.Result?.NotificationLogs?
                    .Where(l => l != null && !l.IsAcknowledged)
                    .ToList() ?? new List<NotificationLogModel>();

                var stillActiveIds = new HashSet<string>();
                foreach (var log in logs)
                {
                    var item = NotificationListItem.FromLog(log);
                    if (item?.Alerts == null)
                        continue;

                    foreach (var alert in item.Alerts)
                        stillActiveIds.Add($"{alert.AlertType}:{alert.Id}");
                }

                return queued
                    .Where(a => a != null && stillActiveIds.Contains($"{a.AlertType}:{a.Id}"))
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Filter queued alerts failed: {ex.Message}");
                return queued;
            }
        }

        public async Task OnPageDisappearingAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            var manager = AppNotificationManager.Instance;
            manager.LiveAlertsReceived -= OnLiveAlertsReceived;
            manager.LeaveColDelList();

            // Unfinished popups are discarded and will not reappear on return.
            await _alertPresenter.DismissActivePopupAsync();
        }

        private void OnLiveAlertsReceived(IList<CollectionAlertItem> alerts)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                    await _alertPresenter.ShowAlertsAsync(
                        alerts,
                        () => LoadList(SelectedFilter == 0 ? (int?)0 : null),
                        syncNotificationCenter: true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Live alert display failed: {ex.Message}");
                }
            });
        }

        #region Commands
        public ICommand RefuseCommand => new Command<DriverCollection>(Refuse);
        public ICommand ClosePopupCommand => new Command(ClosePopup);
        public ICommand AllCommand => new Command(() =>
        {
            SelectedFilter = 3;
            LoadList();
        });

        public ICommand DoneCommand => new Command(() =>
        {
            SelectedFilter = 0;
            LoadList(0);
        });
        #endregion
        #region Methods
        private async Task LoadList(int? status = null)
        {
            try
            {
                IsLoading = true;
                var response = await _shipmentService.DriverCollection(status);

                if (response != null && !response.HasError && response.Result != null)
                {
                    ColDelList.Clear();
                    foreach (var collection in response.Result.Collections)
                    {
                        ColDelList.Add(collection);
                    }
                }
                
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void Refuse(DriverCollection selectedItem)
        {
            await PopupNavigation.Instance.PushAsync(new Refuse(selectedItem));
        }

        public async void NoteToDepot(DriverCollection selectedItem)
        {
            await PopupNavigation.Instance.PushAsync(new DriverNotes(selectedItem));
        }

        public async void Accept(DriverCollection selectedItem)
        {
            try
            {
                this.IsLoading = true;
                var obj = new AcceptCollectionRequestModel
                {
                    id = selectedItem.Id,
                };
                var response = await _shipmentService.AcceptCollection(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Accept Successfully"));
                    LoadList();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void AcceptCollection(DriverCollection selectedItem)
        {
            try
            {
                this.IsLoading = true;
                var obj = new AcceptCollectionRequestListModel
                {
                    ids = new List<int> { selectedItem.Id }
                };
                var response = await _shipmentService.AcceptCollections(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Accept Successfully"));
                    LoadList();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void RefuseColleciton(DriverCollection selectedItem)
        {
            await PopupNavigation.Instance.PushAsync(
                new RefuseCollections(selectedItem));
        }

        public async void CompleteColleciton(DriverCollection selectedItem)
        {
            try
            {
                this.IsLoading = true;
                var obj = new CompleteCollectionRequestModel
                {
                    id = selectedItem.Id,
                };
                var response = await _shipmentService.CompleteCollection(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Complete Successfully"));
                    LoadList();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void NothingToColleciton(DriverCollection selectedItem)
        {
            try
            {
                this.IsLoading = true;
                var obj = new CompleteCollectionRequestModel
                {
                    id = selectedItem.Id,
                };
                var response = await _shipmentService.NothingToCollection(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Updated Successfully"));
                    LoadList();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void DeclineColleciton(DriverCollection selectedItem)
        {
            try
            {
                this.IsLoading = true;
                var obj = new CompleteCollectionRequestModel
                {
                    id = selectedItem.Id,
                };
                var response = await _shipmentService.DeclineCollections(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Decline Successfully"));
                    LoadList();

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}
