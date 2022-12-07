using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportDamage : ContentPage
    {
        public ReportDamage()
        {
            InitializeComponent();
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExemptionScreen());
        }

        private async void ReportDamage_Clicked(object sender, EventArgs e)
        {
            var hold = await Application.Current.MainPage.DisplayAlert("Damage Report!", "Are you sure to Report Damage", "yes", "no");
            if (hold)
            {
                await Navigation.PushAsync(new ExemptionScreen());
            }
        }

        private async void OpenGallery_Tapped(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                imageframe.IsVisible = true;
                resultImage.Source = ImageSource.FromStream(() => stream);
            }
        }
    }
}