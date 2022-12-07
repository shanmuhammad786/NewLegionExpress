using legionexpress.Services;
using legionexpress.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace legionexpress
{
    public partial class App : Application
    {
        //public class MessageKeys
        //{
        //    public const string OnStart = nameof(OnStart);
        //    public const string OnSleep = nameof(OnSleep);
        //    public const string OnResume = nameof(OnResume);
        //}
        private static string appKey = "AT7xGzWXFO6EA8TRlzw8LJ057dQ1G0VzHlxMaPlWSMFvR2oR+VLvDjwZhZiHe0oDUTFyMxJUB2jwJt3KQSKo+lh9vWmEWWPyUzOsd8JbPuipaOGTgnjJwIMifxcTSiXKiUV4vwYzn/gKOLpjSURpS2QbNBJwdAmk/tmveTQ8jDfLxKYwH6Dn846INuDwLGbedm5gdGRhvpsQqUVpnKtUM5NWi852RetGsCA2rw1Ys/sF+9FFs1X8wRh1sfJCXXSzVB5P/dfeWREcNO9wN35Skti2noCHMxGTYqpt1Xi2aZU8U0a3seokJFhxkAUIHm2DAd6WCWUMXbiSOcbzi4/sUK8AMBKPNh+9+op5KxkywtqZdzCvifunUikKSbSXZxPqJK8mfXbg0JeVutj3PqEIMNC8uOZFi/+Exz4qSxvuunVZE5Ms3ruLs+PA/3pFak0Zne0T7kFpgUMFCoa8fqZmrl9Pbfq1IQVXX+sX/Gxlexo+ZYeZherlpf53pxpPxv62kIQlA4DhHen7xCqMAdJaTxiduxCtm02KdXLu5oOtmblHpfUJDCSTKZdVGGRb1H4jmo8z0iRlGMkZPNNrtHd1CEn/Sf5FHC5F+fwkS/Ace85sQmVXqqidOAfnLYMvFLTTooOslq+nziWetJeM0tESkGadqWopK1ZknEmkyBkZ988iiKmQBSac+0jmnX/qTIpQJ4inBe+XBWaCixQppVeMAB2CRJVvwTiLepu9C8orprAG7IgWdp8Ks3HlG6ExGHURIeT0sRnvucNJaQWMZVQtXV93i6GopkYZRK/5z5nNIz4z/NBOkl3zEHReq0bWWrGtaD5zK1A=";
        public App()
        {
            //ScanditService.ScanditLicense.AppKey = appKey;
            //InitSettings();
            InitializeComponent();
            var token = Preferences.Get("token", "default_value");
            if(token != "default_value")
            {
                MainPage = new NavigationPage(new Home());
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
            //DependencyService.Register<IMessageService, MessageService>();
        }
     
        protected override void OnStart()
        {
            //MessagingCenter.Send(this, MessageKeys.OnStart);
            Preferences.Remove("ConsignmentKey");
        }

        protected override void OnSleep()
        {
            //MessagingCenter.Send(this, MessageKeys.OnSleep);

        }

        protected override void OnResume()
        {
            //MessagingCenter.Send(this, MessageKeys.OnResume);

        }
        //async void InitSettings()
        //{
        //    IBarcodePicker picker = ScanditService.BarcodePicker;

        //    // The scanning behavior of the barcode picker is configured through scan
        //    // settings. We start with empty scan settings and enable a very generous
        //    // set of symbologies. In your own apps, only enable the symbologies you
        //    // actually need.
        //    var settings = picker.GetDefaultScanSettings();
        //    settings.CameraPositionPreference = CameraPosition.Front;
        //    settings.RestrictedAreaScanningEnabled = true;
        //    var symbologiesToEnable = new Symbology[] {
        //        Symbology.Qr,
        //        Symbology.Ean13,
        //        Symbology.Upce,
        //        Symbology.Ean8,
        //        Symbology.Upca,
        //        Symbology.Qr,
        //        Symbology.DataMatrix,
        //        Symbology.Code128
        //    };
        //    foreach (var sym in symbologiesToEnable)
        //        settings.EnableSymbology(sym, true);
        //    await picker.ApplySettingsAsync(settings);
        //    // This will open the scanner in full-screen mode. 
        //}
    }
}
