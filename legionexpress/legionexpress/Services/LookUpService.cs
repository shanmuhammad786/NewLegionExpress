using legionexpress.Constants;
using legionexpress.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.Services
{
    public class LookUpService
    {
        internal async Task<LookUpModel> GetNetworks()
        {
            LookUpModel data = null;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string url = "api/v1/lookup/get-all-networks";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(AppConstants.BaseUrl + url);
                    request.Method = HttpMethod.Get;
                    var token = Preferences.Get("token", "default_value");
                    request.Headers.Add("X-ApiKey", token);
                    responseMessage = await httpClient.SendAsync(request);
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<LookUpModel>(response);
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return data;
                    }
                }
                catch (Exception exp)
                {
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "You do not have access to this resource. Invalid API Key!", "Ok");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Alert", exp.ToString(), "Ok");
                }
            }
            //try
            //{
            //    string url = "api/v1/lookup/get-all-networks";
            //    var httpClient = new HttpClient();
            //    httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
            //    var result = await httpClient.GetAsync(url);
            //    var response = await result.Content.ReadAsStringAsync();
            //    data = JsonConvert.DeserializeObject<LookUpModel>(response);
            //    if (result.StatusCode == System.Net.HttpStatusCode.OK)
            //    {
            //        return data;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Alert", ex.ToString(), "Ok");
            //}
            return data;

        }
        internal async Task<ScanResponseModel> ChangeNetworks(Object networkObj)
        {
            ScanResponseModel data = null;
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            string url = "api/v1/shipment/request-update-shipment-network";
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(networkObj));

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(AppConstants.BaseUrl + url);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Content = content;
                    request.Method = HttpMethod.Post;
                    var token = Preferences.Get("token", "default_value");
                    request.Headers.Add("X-ApiKey", token);
                    responseMessage = await httpClient.SendAsync(request);
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Something went wrong please try again", "Ok");
                        return data;
                    }
                    return data;
                }
                catch (Exception exp)
                {
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "You do not have access to this resource. Invalid API Key!", "Ok");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Alert", exp.ToString(), "Ok");
                    throw exp;
                }
            }
        }
    }
}
