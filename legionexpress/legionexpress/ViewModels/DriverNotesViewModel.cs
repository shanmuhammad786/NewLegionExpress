using System;
using System.Threading.Tasks;
using System.Windows.Input;
using legionexpress.Models;
using legionexpress.Popups;
using legionexpress.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace legionexpress.ViewModels
{
	public class DriverNotesViewModel : BaseViewModel
    {
        #region PrivateProperties
        private bool _isLoading;
        private string _notes;
        private readonly ShipmentService _shipmentService;
        #endregion
        #region PublicProperties
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public DriverCollection SelectedItem { get; }

        public DriverNotesViewModel(DriverCollection item)
		{
            _shipmentService = new ShipmentService();
            SelectedItem = item;
            Notes = SelectedItem.Notes;
        }

        #region commands
        public ICommand ClosePopupCommand => new Command(ClosePopup);
        public ICommand SubmitPopupCommand => new Command(() =>
        {
            OnDeclineSubmit();
        });
        #endregion
        #region methods
        private async Task OnDeclineSubmit()
        {
            try
            {
                IsLoading = true;
                var obj = new DriverNotesRequestModel
                {
                    id = SelectedItem.Id,
                    notes = Notes 
                };
                var response = await _shipmentService.UpdateDriverNotes(obj);

                if (response != null && !response.HasError && response.Result != null)
                {
                    MessagingCenter.Send<object, bool>(this, "RefreshList", true);
                    await PopupNavigation.Instance.PopAsync();
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
        public async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}

