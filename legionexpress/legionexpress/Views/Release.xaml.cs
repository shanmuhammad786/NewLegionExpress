using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Release : ContentPage
    {
        public Release()
        {
            InitializeComponent();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExemptionScreen());
        }

        private async void Release_Clicked(object sender, EventArgs e)
        {
            var release = await Application.Current.MainPage.DisplayAlert("Release Consignment!", "Are you sure to Release consignment", "yes", "no");
            if (release)
            {
                await Navigation.PushAsync(new ExemptionScreen());
            }
        }
    }
}