using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading.Tasks;

namespace legionexpress.Droid.Activities
{
    [Activity(Theme = "@style/Theme.Splash",
        MainLauncher = false,
        NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
            // Splash is no longer the launcher; forward immediately if opened.
            StartActivity(new Intent(this, typeof(MainActivity)));
            Finish();
        }
    }
}
