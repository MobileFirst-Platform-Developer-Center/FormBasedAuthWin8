/**
* Copyright 2015 IBM Corp.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IBM.Worklight;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Windows.UI.Core;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FormBasedAuthWin8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WLClient wlClient = null;
        public static MainPage _this;
        FormChallengeHandler ch;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            _this = this;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        public ChallengeHandler getChallengeHandler()
        {
            return ch;
        }


        private void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     MainPage._this.Console.Text = "\n\nConnecting to server";

                 });


                wlClient = WLClient.getInstance();

                ch = new FormChallengeHandler();
                wlClient.registerChallengeHandler((BaseChallengeHandler<JObject>)ch);

                MyResponseListener mylistener = new MyResponseListener(this);
                wlClient.connect(mylistener);
            }
            catch (Exception ex)
            { Debug.WriteLine(ex.StackTrace); }
        }

        private void invokeProcedure_Click(object sender, RoutedEventArgs e)
        {
            WLResourceRequest adapter = new WLResourceRequest("/adapters/AuthAdapter/getSecretData", "GET");

            Object[] parameters = { 0 };
            MyInvokeListener listener = new MyInvokeListener(this);
            adapter.send(listener);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ((FormChallengeHandler)MainPage._this.getChallengeHandler()).sendResponse(username.Text, password.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                MainPage._this.LoginGrid.Visibility = Visibility.Collapsed;
                MainPage._this.ConnectServer.IsEnabled = true;
                MainPage._this.InvokeProcedure.IsEnabled = true;

            });
            ((FormChallengeHandler)MainPage._this.getChallengeHandler()).sendResponse("", "");
        }

        public void AddTextToConsole(String consoleText)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     MainPage._this.Console.Text = consoleText;

                 });
        }


        public void showChallenge()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     MainPage._this.LoginGrid.Visibility = Visibility.Visible;
                     MainPage._this.ConnectServer.IsEnabled = false;
                     MainPage._this.InvokeProcedure.IsEnabled = false;
                 });
        }

        public void hideChallenge()
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    MainPage._this.LoginGrid.Visibility = Visibility.Collapsed;
                    MainPage._this.ConnectServer.IsEnabled = true;
                    MainPage._this.InvokeProcedure.IsEnabled = true;
                });
        }

        private void ClearConsole(object sender, DoubleTappedRoutedEventArgs e)
        {
            Console.Text = "";
        }
    }
}
