using legionexpress.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCollectionRequestPopup : PopupPage
    {
        private readonly List<CollectionAlertItem> _alerts = new List<CollectionAlertItem>();
        private readonly Func<CollectionAlertItem, Task<bool>> _onAccept;
        private readonly Func<CollectionAlertItem, Task<bool>> _onCancel;
        private readonly Func<CollectionAlertItem, Task<bool>> _onAcknowledgeNotes;
        private readonly Action _onClosed;
        private bool _isProcessing;

        public NewCollectionRequestPopup(
            CollectionAlertItem firstAlert,
            Func<CollectionAlertItem, Task<bool>> onAccept,
            Func<CollectionAlertItem, Task<bool>> onCancel,
            Func<CollectionAlertItem, Task<bool>> onAcknowledgeNotes,
            Action onClosed = null)
        {
            InitializeComponent();
            _onAccept = onAccept;
            _onCancel = onCancel;
            _onAcknowledgeNotes = onAcknowledgeNotes;
            _onClosed = onClosed;
            _alerts.Add(firstAlert);
            BindCurrentAlert();
        }

        public void Enqueue(CollectionAlertItem alert)
        {
            if (alert == null)
                return;

            if (ContainsAlert(alert))
                return;

            _alerts.Add(alert);
            BindCurrentAlert();
        }

        public bool ContainsAlert(CollectionAlertItem alert)
        {
            if (alert == null)
                return false;

            return _alerts.Any(x => x.Id == alert.Id && x.AlertType == alert.AlertType);
        }

        private void BindCurrentAlert()
        {
            var current = _alerts.FirstOrDefault();
            if (current == null)
                return;

            var isNotes = current.IsNotesAlert;

            TitleLabel.Text = isNotes ? "New Message Request" : "New Collection Request";
            CustomerNameLabel.Text = string.IsNullOrWhiteSpace(current.CustomerName)
                ? $"Collection #{current.Id}"
                : current.CustomerName;
            PostcodeLabel.Text = current.Postcode ?? string.Empty;

            NotesLabel.IsVisible = isNotes && !string.IsNullOrWhiteSpace(current.Notes);
            NotesLabel.Text = current.Notes ?? string.Empty;

            CancelButton.IsVisible = !isNotes;
            if (isNotes)
            {
                Grid.SetColumnSpan(OkButton, 2);
                ActiveCard.HeightRequest = 260;
                PopupRoot.HeightRequest = 320;
            }
            else
            {
                Grid.SetColumnSpan(OkButton, 1);
                ActiveCard.HeightRequest = 220;
                PopupRoot.HeightRequest = 280;
            }

            var remainingBehind = Math.Max(0, _alerts.Count - 1);
            StackCard1.IsVisible = remainingBehind >= 1;
            StackCard2.IsVisible = remainingBehind >= 2;
            StackCard3.IsVisible = remainingBehind >= 3;
        }

        private async void Ok_Clicked(object sender, EventArgs e)
        {
            var current = _alerts.FirstOrDefault();
            if (current == null)
            {
                await ClosePopupAsync();
                return;
            }

            if (current.IsNotesAlert)
                await HandleActionAsync(_onAcknowledgeNotes);
            else
                await HandleActionAsync(_onAccept);
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await HandleActionAsync(_onCancel);
        }

        private async Task HandleActionAsync(Func<CollectionAlertItem, Task<bool>> action)
        {
            if (_isProcessing || action == null)
                return;

            var current = _alerts.FirstOrDefault();
            if (current == null)
            {
                await ClosePopupAsync();
                return;
            }

            try
            {
                _isProcessing = true;
                SetButtonsEnabled(false);

                var success = await action(current);
                if (!success)
                    return;

                _alerts.RemoveAt(0);

                if (_alerts.Count == 0)
                {
                    await ClosePopupAsync();
                    return;
                }

                BindCurrentAlert();
            }
            finally
            {
                _isProcessing = false;
                SetButtonsEnabled(true);
            }
        }

        private void SetButtonsEnabled(bool enabled)
        {
            if (OkButton != null)
                OkButton.IsEnabled = enabled;
            if (CancelButton != null)
                CancelButton.IsEnabled = enabled;
        }

        private async Task ClosePopupAsync()
        {
            _alerts.Clear();
            StackCard1.IsVisible = false;
            StackCard2.IsVisible = false;
            StackCard3.IsVisible = false;

            _onClosed?.Invoke();

            if (PopupNavigation.Instance.PopupStack.Contains(this))
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }
    }
}
