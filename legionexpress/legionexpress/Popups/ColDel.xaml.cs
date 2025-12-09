using legionexpress.Models;
using legionexpress.ViewModels;
using legionexpress.Views;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColDel : PopupPage
    {
        public ColDel()
        {
            InitializeComponent();
            BindingContext = new ColDelViewModel();
        }

        //private void Refuse_Tapped(object sender, EventArgs e)
        //{
        //    var vm = BindingContext as ColDelViewModel;
        //    vm.Refuse();
        //}
        private void Accept_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.Accept(selectedItem);
            }
            //var vm = BindingContext as ColDelViewModel;
            //vm.Accept();
        }

        private void Refuse_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is DriverCollection selectedItem)
            {
                var vm = BindingContext as ColDelViewModel;
                vm?.Refuse(selectedItem);
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
            //var data = sender as Image;
            //var index = data.BindingContext as ColDelModel;
            //if(index != null)
            //{
            //    if(index.Type == "COL")
            //    {
            //        await this.Navigation.PushAsync(new ScanditCollectionScanning());

            //    }
            //    if (index.Type == "DEL")
            //    {
            //        await this.Navigation.PushAsync(new DeliveryScanning());

            //    }
            //}
        }
    }
}