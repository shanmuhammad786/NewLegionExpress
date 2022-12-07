using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace legionexpress.Services
{
    public class MessageService : IMessageService
    {
        public Task ShowAsync(string message, Action handler = null)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                var alertStatus = App.Current.MainPage.DisplayAlert("Scandit", message, "OK");
                alertStatus.ContinueWith((Task) => handler?.Invoke());
            });
        }
    }
}
