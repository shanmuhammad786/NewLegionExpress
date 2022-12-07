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
    public partial class RTS : ContentPage
    {
        public RTS()
        {
            InitializeComponent();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExemptionScreen());
        }

        private async void Return_Clicked(object sender, EventArgs e)
        {
            var RTS = await Application.Current.MainPage.DisplayAlert("Return to Sender", "Are you sure to Return consignment", "yes", "no");
            if (RTS)
            {
                await Navigation.PushAsync(new ExemptionScreen());
            }
        }
    }
}