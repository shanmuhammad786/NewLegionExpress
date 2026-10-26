using legionexpress.Models;
using legionexpress.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        private NotificationsViewModel ViewModel => BindingContext as NotificationsViewModel;

        public NotificationsPage()
        {
            InitializeComponent();
            BindingContext = new NotificationsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel == null)
                return;

            ViewModel.StartListeningForSignalR();
            await ViewModel.LoadAsync();
        }

        protected override void OnDisappearing()
        {
            ViewModel?.StopListeningForSignalR();
            base.OnDisappearing();
        }

        private async void NotificationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            var item = e.CurrentSelection[0] as NotificationListItem;
            NotificationsList.SelectedItem = null;

            if (item != null && ViewModel != null)
                await ViewModel.OpenNotificationAsync(item);
        }
    }
}
