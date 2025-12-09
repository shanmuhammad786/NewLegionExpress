using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Net;

namespace legionexpress.Droid
{
    [Activity(Label = "legionexpress", Icon = "@drawable/appIcon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            //MessagingCenter.Subscribe<string>("1", "GetToken", (sender) =>
            //{
            //    FirbaseGetInstance();
            //});
           // FirbaseGetInstance();
           // IsPlayServicesAvailable();
            LoadApplication(new App());
            Xamarin.Forms.Application.Current
                .On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //private void FirbaseGetInstance()
        //{
        //   // FirebaseApp.InitializeApp(this);
        //    var token = FirebaseInstanceId.Instance.Token;
        //    Utilty.FirebaseToken = FirebaseInstanceId.Instance.Token;

        //}
        //private bool IsPlayServicesAvailable()
        //{
        //    int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
        //    if (resultCode != ConnectionResult.Success)
        //    {
        //        if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
        //        {
        //            var msgTest = GoogleApiAvailability.Instance.GetErrorString(resultCode);
        //        }
        //        else
        //        {
        //            var msgTest = "This device is not supported";
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        var msgTest = "Google Api service Available";
        //        return true;
        //    }
        //}
    }
}