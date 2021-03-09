using IdentityModel.OidcClient;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Windows.Controls;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClient _oidcClient = null;
        private LoginResult _result;
        private ClaimsPrincipal _user;


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
                var myResult = await _oidcClient.LoginAsync();
                _result = await _oidcClient.LoginAsync();
                _user = _result.User;
            }
            catch (Exception ex)
            {
                return;
            }

            if (_result.IsError)
            {

            }
            else
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _result.AccessToken);

                var apiResult = await client.GetStringAsync("https://localhost:44387/secret");
                MessageBox.Show(apiResult);
            }
        }

        [Authorize]
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        [Authorize]
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_user.Identity.IsAuthenticated)
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
