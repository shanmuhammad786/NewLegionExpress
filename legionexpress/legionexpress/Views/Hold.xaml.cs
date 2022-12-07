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
    public partial class Hold : ContentPage
    {
        public Hold()
        {
            InitializeComponent();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExemptionScreen());
        }

        private async void HoldConsignment_Clicked(object sender, EventArgs e)
        {
            var hold = await Application.Current.MainPage.DisplayAlert("Hold Consignment!", "Are you sure to hold consignment", "yes", "no");
            if (hold)
            {
                await Navigation.PushAsync(new ExemptionScreen());
            }
        }
    }
}