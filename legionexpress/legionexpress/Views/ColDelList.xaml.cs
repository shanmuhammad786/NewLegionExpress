using System;
using legionexpress.Models;
using legionexpress.ViewModels;
using Xamarin.Forms;

namespace legionexpress.Views
{	
	public partial class ColDelList : ContentPage
    {
        private ColDelViewModel ViewModel => BindingContext as ColDelViewModel;

		public ColDelList ()
		{
			InitializeComponent ();
            BindingContext = new ColDelViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
                await ViewModel.OnPageAppearingAsync();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
                await ViewModel.OnPageDisappearingAsync();
        }

        private void Accept_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.AcceptCollection(selectedItem);
            }
        }

        private void Refuse_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.RefuseColleciton(selectedItem);
            }
        }

        private void Complete_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.CompleteColleciton(selectedItem);
            }
        }

        private void NothingToCollect_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.NothingToColleciton(selectedItem);
            }
        }

        private void Decline_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.DeclineColleciton(selectedItem);
            }
        }

        private void Notes_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.NoteToDepot(selectedItem);
            }
        }
        private async void scanner_Tapped(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new ScanditCollectionScanning());
        }
        private void MoreInfo_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is DriverCollection selectedItem)
            {
                selectedItem.IsInfoVisible = !selectedItem.IsInfoVisible;
            }
        }
    }
}
