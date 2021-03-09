﻿using IdentityModel;
using IdentityModel.OidcClient;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClient _oidcClient = null;
        private LoginResult result;


        public MainWindow()
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:5001",
                ClientId = Constants.Clients.Wpf,
                Scope = "openid ApiOne profile",
                RedirectUri = "http://localhost/sample-wpf-app",
                Browser = new WpfEmbeddedBrowser()
            };
            _oidcClient = new OidcClient(options);
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                result = await _oidcClient.LoginAsync();
            }
            catch (Exception ex)
            {
                return;
            }

            if (result.IsError)
            {

            }
            else
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                var apiResult = await client.GetStringAsync("https://localhost:44387/secret");
                MessageBox.Show(apiResult);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"UserName is {result.User.Claims.Where(x => x.Type == JwtClaimTypes.Name).FirstOrDefault().Value}");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (result.User.Identity.IsAuthenticated)
            {
                MessageBox.Show("User is authenticated");
            }
            else
            {
                MessageBox.Show("User is not authenticated");
            }
        }
    }
}