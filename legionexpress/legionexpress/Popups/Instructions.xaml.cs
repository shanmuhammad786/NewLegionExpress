using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Instructions : PopupPage
    {
        public Instructions(string lengthAlert, string instructions, string localPostalCode,string worldOptionCode)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(lengthAlert))
            {
                LengthAlert.Text = lengthAlert;
            }
            else
            {
                LengthAlert.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(instructions))
            {
                InstructionsText.Text = instructions;
            }
            else
            {
                Instruction.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(localPostalCode))
            {
                PostalCodeText.Text = localPostalCode;
            }
            else
            {
                PostalCode.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(worldOptionCode))
            {
                WorldOptionCodeText.Text = worldOptionCode;
            }
            else
            {
                WorldOptionCode.IsVisible = false;
            }


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<string>("1", "DisableScan");
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await Task.Delay(500);
            if (PopupNavigation.PopupStack.Count == 0)
                MessagingCenter.Send<string>("1", "EnableScan");
        }
    }
}