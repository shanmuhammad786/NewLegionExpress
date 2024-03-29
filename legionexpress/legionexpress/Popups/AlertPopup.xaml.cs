﻿using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class AlertPopup : PopupPage
    {
        public AlertPopup(string alert, string description)
        {
            InitializeComponent();
            Alert.Text = alert;
            Description.Text = description;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
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