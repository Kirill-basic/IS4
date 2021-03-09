using IdentityModel.OidcClient;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Authentication authentication;

        public MainWindow()
        {
            InitializeComponent();

            authentication = Authentication.GetAuthentication();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await authentication.Login();
        }

        [Authorize]
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (authentication.User?.Identity.IsAuthenticated ?? false)
            {
                MessageBox.Show("authenticated user " + authentication.User.Identity.Name);
            }
            else
            {
                MessageBox.Show("User is unauthenticated");
            }
        }

        [Authorize]
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (authentication.User.Identity.IsAuthenticated)
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
