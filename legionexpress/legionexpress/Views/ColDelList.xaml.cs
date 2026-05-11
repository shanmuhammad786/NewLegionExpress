using System;
using System.Collections.Generic;
using legionexpress.Models;
using legionexpress.ViewModels;
using Xamarin.Forms;

namespace legionexpress.Views
{	
	public partial class ColDelList : ContentPage
	{	
		public ColDelList ()
		{
			InitializeComponent ();
            BindingContext = new ColDelViewModel();
        }
        private void Accept_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                //vm?.Accept(selectedItem);
                vm?.AcceptCollection(selectedItem);
            }
        }

        private void Refuse_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.RefuseColleciton(selectedItem);
                //vm?.Refuse(selectedItem);
            }
        }

        private void Complete_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.CompleteColleciton(selectedItem);
                //vm?.Refuse(selectedItem);
            }
        }

        private void NothingToCollect_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.NothingToColleciton(selectedItem);
                //vm?.Refuse(selectedItem);
            }
        }

        private void Decline_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.DeclineColleciton(selectedItem);
                //vm?.Refuse(selectedItem);
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

