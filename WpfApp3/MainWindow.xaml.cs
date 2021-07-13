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
        private Authentication _authentication;
        public MainWindow()
        {
            _authentication = Authentication.GetAuthentication();
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _authentication.Login();
        }

        [Authorize]
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_authentication.IsAuthenticated)
            {
                MessageBox.Show(_authentication.User.Identity.Name);
            }
            else
            {
                MessageBox.Show("User is not authenticated");
            }
        }

        [Authorize]
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var result = await _authentication.GetRequestAsync();
            MessageBox.Show(result);
        }
    }
}
