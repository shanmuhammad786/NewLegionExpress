using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using Scandit.DataCapture.Barcode.Capture.Unified;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
            ColDelList = new ObservableCollection<DriverCollection>();
            MessagingCenter.Subscribe<object, bool>(this, "RefreshList", HandleRefresh);
            LoadList();
            //LoadList();
        }
        private void HandleRefresh(object sender, bool shouldRefresh)
        {
            if (shouldRefresh)
            {
                LoadList();
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
        public async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}
