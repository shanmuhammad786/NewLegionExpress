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
	public class NewRefuseCollectionsViewModel : BaseViewModel
    {
        private readonly ShipmentService _shipmentService;

        public DriverCollection SelectedItem { get; set; }

        public string RefusedNotes { get; set; }

        public string PersonOnSite { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SubmitPopupCommand => new Command(async () => await Submit());

        public ICommand ClosePopupCommand => new Command(async () => await ClosePopup());

        public NewRefuseCollectionsViewModel(DriverCollection driverCollection)
		{
            SelectedItem = driverCollection;
            _shipmentService = new ShipmentService();
        }

        private async Task Submit()
        {
            try
            {
                IsLoading = true;

                var obj = new RefuseCollectionRequestModel
                {
                    id = SelectedItem.Id,
                    refusedNotes = RefusedNotes,
                    personOnSite = PersonOnSite
                };

                var response = await _shipmentService.RefuseCollection(obj);

                if (response != null && !response.HasError)
                {
                    MessagingCenter.Send<object, bool>(this, "RefreshList", true);

                    await PopupNavigation.Instance.PushAsync(
                        new AlertPopup("Success", "Collection Refused Successfully"));

                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PushAsync(
                    new AlertPopup("Error", ex.Message));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ClosePopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}

