using legionexpress.Constants;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.Services
{
    public class NotificationHubService
    {
        private HubConnection _connection;
        private bool _isConnecting;

        public event Action<string> MessageReceived;

        public bool IsConnected =>
            _connection?.State == HubConnectionState.Connected;

        public async Task ConnectAsync()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            if (_isConnecting || IsConnected)
                return;

            var token = Preferences.Get("token", "default_value");
            if (string.IsNullOrWhiteSpace(token) || token == "default_value")
                return;

            try
            {
                _isConnecting = true;

                if (_connection != null)
                {
                    await DisconnectAsync();
                }

                _connection = new HubConnectionBuilder()
                    .WithUrl($"{AppConstants.BaseUrl}notificationHub", options =>
                    {
                        options.Headers["X-ApiKey"] = token;
                    })
                    .WithAutomaticReconnect()
                    .Build();

                _connection.On<string>("ReceiveMessage", payload =>
                {
                    MessageReceived?.Invoke(payload);
                });

                _connection.Reconnecting += error =>
                {
                    System.Diagnostics.Debug.WriteLine($"SignalR reconnecting: {error?.Message}");
                    return Task.CompletedTask;
                };

                _connection.Reconnected += connectionId =>
                {
                    System.Diagnostics.Debug.WriteLine($"SignalR reconnected: {connectionId}");
                    return Task.CompletedTask;
                };

                _connection.Closed += error =>
                {
                    System.Diagnostics.Debug.WriteLine($"SignalR closed: {error?.Message}");
                    return Task.CompletedTask;
                };

                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SignalR connect failed: {ex.Message}");
                await DisconnectAsync();
            }
            finally
            {
                _isConnecting = false;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_connection == null)
                return;

            try
            {
                await _connection.StopAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SignalR stop failed: {ex.Message}");
            }

            try
            {
                await _connection.DisposeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SignalR dispose failed: {ex.Message}");
            }

            _connection = null;
        }
    }
}
