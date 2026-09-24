using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Newtonsoft.Json;
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
        private readonly NotificationHubService _notificationHubService;
        private NewCollectionRequestPopup _collectionAlertPopup;
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
                // Also notify changes for button styles
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
        //new Setter { Property = Button.BackgroundColorProperty, Value = Color.Black },
        //new Setter { Property = Button.TextColorProperty, Value = Color.White }
        new Setter { Property = Button.BackgroundColorProperty, Value = "#2E2E2E" },
        new Setter { Property = Button.TextColorProperty, Value = Color.White }
    }
        });

        private Style _unselectedButtonStyle;
        public Style UnselectedButtonStyle => _unselectedButtonStyle ?? (_unselectedButtonStyle = new Style(typeof(Button))
        {
            Setters = {
        //new Setter { Property = Button.BackgroundColorProperty, Value = Color.White },
        //new Setter { Property = Button.TextColorProperty, Value = Color.Black },
        //new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
        //new Setter { Property = Button.BorderWidthProperty, Value = 1 }
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
            _notificationHubService = new NotificationHubService();
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

        public async Task StartNotificationsAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            _notificationHubService.MessageReceived -= OnNotificationReceived;
            _notificationHubService.MessageReceived += OnNotificationReceived;
            await _notificationHubService.ConnectAsync();
        }

        public async Task StopNotificationsAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            _notificationHubService.MessageReceived -= OnNotificationReceived;
            await _notificationHubService.DisconnectAsync();
        }

        private void OnNotificationReceived(string payload)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await HandleNotificationAsync(payload);
            });
        }

        private async Task HandleNotificationAsync(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return;

            try
            {
                var notification = JsonConvert.DeserializeObject<NotificationPayloadModel>(payload);
                if (notification?.Data == null || string.IsNullOrWhiteSpace(notification.Event))
                    return;

                List<CollectionAlertItem> alerts;
                if (string.Equals(notification.Event, "CollectionAssigned", StringComparison.OrdinalIgnoreCase))
                {
                    alerts = BuildCollectionAssignedAlerts(notification.Data);
                }
                else if (string.Equals(notification.Event, "NewCollectionNotes", StringComparison.OrdinalIgnoreCase))
                {
                    alerts = BuildNewCollectionNotesAlerts(notification.Data);
                }
                else
                {
                    return;
                }

                if (alerts.Count == 0)
                    return;

                // Refresh list so updated collections/notes appear behind the popups.
                await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                await ShowCollectionAlertsAsync(alerts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification handling failed: {ex.Message}");
            }
        }

        private static List<CollectionAlertItem> BuildCollectionAssignedAlerts(NotificationDataModel data)
        {
            var alerts = new List<CollectionAlertItem>();

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
                        AlertType = CollectionAlertType.CollectionAssigned
                    });
                }

                return alerts;
            }

            // Fallback for older payloads that only sent a single collection id.
            if (data.Id > 0)
            {
                alerts.Add(new CollectionAlertItem
                {
                    Id = data.Id,
                    AlertType = CollectionAlertType.CollectionAssigned
                });
            }

            return alerts;
        }

        private static List<CollectionAlertItem> BuildNewCollectionNotesAlerts(NotificationDataModel data)
        {
            var alerts = new List<CollectionAlertItem>();
            var collection = data.Collection;
            if (collection == null || collection.Id <= 0)
                return alerts;

            alerts.Add(new CollectionAlertItem
            {
                Id = collection.Id,
                CustomerName = collection.CustomerName,
                Postcode = collection.PostCode,
                Notes = collection.Notes,
                AlertType = CollectionAlertType.NewCollectionNotes
            });

            return alerts;
        }

        private async Task ShowCollectionAlertsAsync(IList<CollectionAlertItem> alerts)
        {
            if (alerts == null || alerts.Count == 0)
                return;

            var newAlerts = alerts
                .Where(a => a != null && a.Id > 0)
                .Where(a => _collectionAlertPopup == null || !_collectionAlertPopup.ContainsAlert(a))
                .GroupBy(a => new { a.Id, a.AlertType })
                .Select(g => g.First())
                .ToList();

            if (newAlerts.Count == 0)
                return;

            if (_collectionAlertPopup != null)
            {
                foreach (var alert in newAlerts)
                    _collectionAlertPopup.Enqueue(alert);
                return;
            }

            _collectionAlertPopup = new NewCollectionRequestPopup(
                newAlerts[0],
                AcceptCollectionFromAlertAsync,
                DeclineCollectionFromAlertAsync,
                AcknowledgeNotesFromAlertAsync,
                () => _collectionAlertPopup = null);

            for (var i = 1; i < newAlerts.Count; i++)
                _collectionAlertPopup.Enqueue(newAlerts[i]);

            await PopupNavigation.Instance.PushAsync(_collectionAlertPopup);
        }

        /// <summary>
        /// OK on SignalR collection alert — same accept API used by the Col/Del list.
        /// </summary>
        private async Task<bool> AcceptCollectionFromAlertAsync(CollectionAlertItem alert)
        {
            try
            {
                IsLoading = true;
                var request = new AcceptCollectionRequestListModel
                {
                    ids = new List<int> { alert.Id }
                };
                var response = await _shipmentService.AcceptCollections(request);

                if (response != null && !response.HasError)
                {
                    await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                    return true;
                }

                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", response?.ErrorMessage ?? "Unable to accept collection"));
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Accept from alert failed: {ex.Message}");
                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", "Unable to accept collection"));
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Cancel on SignalR collection alert — same decline API used by the Col/Del list.
        /// </summary>
        private async Task<bool> DeclineCollectionFromAlertAsync(CollectionAlertItem alert)
        {
            try
            {
                IsLoading = true;
                var request = new CompleteCollectionRequestModel
                {
                    id = alert.Id
                };
                var response = await _shipmentService.DeclineCollections(request);

                if (response != null && !response.HasError)
                {
                    await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                    return true;
                }

                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", response?.ErrorMessage ?? "Unable to decline collection"));
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Decline from alert failed: {ex.Message}");
                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", "Unable to decline collection"));
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// OK on notes alert — acknowledge only, then refresh list.
        /// </summary>
        private async Task<bool> AcknowledgeNotesFromAlertAsync(CollectionAlertItem alert)
        {
            try
            {
                await LoadList(SelectedFilter == 0 ? (int?)0 : null);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Acknowledge notes failed: {ex.Message}");
                return true;
            }
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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }
        //public async void Refuse()
        //{
        //    await PopupNavigation.Instance.PushAsync(new Refuse() { BindingContext = this});
        //}

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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
        }

        public async void RefuseColleciton(DriverCollection selectedItem)
        {
            //try
            //{
            //    this.IsLoading = true;
            //    var obj = new CompleteCollectionRequestModel
            //    {
            //        id = selectedItem.Id,
            //    };
            //    var response = await _shipmentService.RefuseCollection(obj);

            //    if (response != null && !response.HasError && response.Result != null)
            //    {
            //        await PopupNavigation.Instance.PushAsync(new AlertPopup("Success", "Collection Refuse Successfully"));
            //        LoadList();

            //    }
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    IsLoading = false;
            //}

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
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
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }

            //await PopupNavigation.Instance.PushAsync(new Accept() { BindingContext = this });
        }

        public async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}
