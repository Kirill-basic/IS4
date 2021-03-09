using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class Authentication
    {
        private static Authentication AuthenticationInstance;
        private OidcClient _client;
        private LoginResult _loginResult;
        public ClaimsPrincipal User;

        private Authentication()
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:5001",
                ClientId = Constants.Clients.Wpf,
                Scope = "openid ApiOne profile",
                RedirectUri = "http://localhost/sample-wpf-app",
                Browser = new WpfEmbeddedBrowser()
            };

            _client = new OidcClient(options);
        }


        public static Authentication GetAuthentication()
        {
            if (AuthenticationInstance == null)
            {
                AuthenticationInstance = new Authentication();
            }

            return AuthenticationInstance;
        }


        public async Task Login()
        {
            var result = await _client.PrepareLoginAsync();
            _loginResult = await _client.LoginAsync();
            User = _loginResult.User;
        }
    }
}
