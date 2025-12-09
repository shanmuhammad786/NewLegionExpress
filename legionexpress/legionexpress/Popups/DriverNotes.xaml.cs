using legionexpress.Models;
using legionexpress.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverNotes : PopupPage
    {

        public DriverNotes(DriverCollection driverCollection)
        {
            InitializeComponent();
            BindingContext = new DriverNotesViewModel(driverCollection);
        }
    }
}