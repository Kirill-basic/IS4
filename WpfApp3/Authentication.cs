using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace WpfApp3
{
    class Authentication
    {
        private static Authentication AuthenticationInstance;
        private OidcClient _client;
        private LoginResult _loginResult;
        private ClaimsPrincipal User;

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



    }
}
