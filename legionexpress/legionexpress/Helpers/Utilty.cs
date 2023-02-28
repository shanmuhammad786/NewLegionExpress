using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace legionexpress.Helpers
{
    public class Utilty
    {
        public static string HoldandReleaseKey = "Hold";
        public static int CollectionScanCount = 0;
        public static int WarehouseScanCount = 0;
        public static int count = 0;
        public static string username = string.Empty;

        public static async Task SetSecureStorageValue(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                App.Current.Properties[key] = value;
            }

        }

        public static async Task<string> GetSecureStorageValueFor(string key)
        {
            string value = "";
            try
            {
                value = await SecureStorage.GetAsync(key);
            }
            catch (Exception ex)
            {
                value = App.Current.Properties[key] as string;
            }

            return value;
        }
    }
}
