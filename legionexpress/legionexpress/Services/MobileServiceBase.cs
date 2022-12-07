using legionexpress.Constants;
using legionexpress.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace legionexpress.Services
{
    internal abstract class MobileServiceBase
    {
        protected async Task<T> PostUnauthorized<T, X>(X obj, string url)
        {
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
                try
                {
                    httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(url, content);

                    var response = await result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(response);

                    if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
                    {
                        return data;
                    }

                    return data;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        protected async Task<T> PostAuthorized<T>(T obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

            using (var httpClient = new HttpClient())
            {
                // x-api-key is not working properly
                //var authHeader = new AuthenticationHeaderValue("X-ApiKey", Preferences.Get("token", "default_value"));
                //httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
                //httpClient.DefaultRequestHeaders.Authorization = authHeader;
                

                try
                {
                    // httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
                    //var result = await httpClient.PostAsync(url, content);
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(AppConstants.BaseUrl + url);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Content = content;
                    request.Method = HttpMethod.Post;
                    var token = Preferences.Get("token", "default_value");
                    request.Headers.Add("X-ApiKey", token);
                    responseMessage = await httpClient.SendAsync(request);
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(response);
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
        protected async Task<ScanResponseModel> AddNote(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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
        protected async Task<ScanResponseModel> HoldShipment(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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
        protected async Task<ScanResponseModel> UpdateDimenstions(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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
        protected async Task<ScanResponseModel> GetScannedShipment( string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);

                    if (responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == HttpStatusCode.OK)
                    {
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
        protected async Task<ScanResponseModel> SetScannedShipment(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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
        protected async Task<ScanResponseModel> ReleaseShipment(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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
        protected async Task<ScanResponseModel> LabelReprint(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ScanResponseModel>(response);
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

        protected async Task<T> GetAuthorized<T>(string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    // x-api-key is not working properly
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(AppConstants.BaseUrl+url);
                    request.Method = HttpMethod.Get;
                    var token = Preferences.Get("token", "default_value");
                    request.Headers.Add("X-ApiKey",token);
                    responseMessage = await httpClient.SendAsync(request);
                    // var authHeader = new AuthenticationHeaderValue("x-api-key", Preferences.Get("token", "default_value"));
                    // httpClient.BaseAddress = new Uri(AppConstants.BaseUrl);
                    // httpClient.DefaultRequestHeaders.Authorization = authHeader;

                    //var result = await httpClient.GetAsync(url);
                    var response = await responseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(response);

                    if (responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return data;

                    }
                    else
                    {
                        return default(T);
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
                    return default(T);
                }
            }
        }

        protected async Task<ShipmentDetailsModel> PostShipmentCollectionScanned<T>(Object obj, string url)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var requestBody = await Task.Run(() => JsonConvert.SerializeObject(obj));

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
                    var data = JsonConvert.DeserializeObject<ShipmentDetailsModel>(response);
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert",data.errorMessage , "Ok");
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
