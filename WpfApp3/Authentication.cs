using Constants;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WpfApp3
{
    class Authentication
    {
        private static Authentication _authenticationInstance;
        private OidcClient _client;
        public ClaimsPrincipal User;
        private Credentials _credentials;

        public bool IsAuthenticated
        {
            get
            {
                return User?.Identity.IsAuthenticated ?? false;
            }
        }

        private Authentication()
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:5001",
                ClientId = Clients.Wpf,
                Scope = "openid profile ApiOne offline_access",
                RedirectUri = "http://localhost/sample-wpf-app",
                Browser = new WpfEmbeddedBrowser()
            };

            _client = new OidcClient(options);
        }


        public static Authentication GetAuthentication()
        {
            if (_authenticationInstance == null)
            {
                _authenticationInstance = new Authentication();
            }

            return _authenticationInstance;
        }


        public async Task Login()
        {
            var loginResult = await _client.LoginAsync();
            _credentials = loginResult.ToCredentials();
            User = loginResult.User;
        }

        public async Task LogOut()
        {
            var logoutResult = await _client.LogoutAsync();
        }

        public async Task<string> GetRequestAsync()
        {
            var client = new HttpClient();
            try
            {
                var token = _credentials.AccessToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _credentials.AccessToken);
                var apiResult = await client.GetStringAsync("https://localhost:8001/secret");
                return apiResult;
            }
            catch (Exception e)
            {
                await RefreshTokenAsync();

                //old token is received here
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _credentials.AccessToken);
                var apiResult = await client.GetStringAsync("https://localhost:8001/secret");
                return apiResult;
            }
        }

        private async Task RefreshTokenAsync()
        {
            var refreshToken = _credentials.RefreshToken;
            var refreshTokenResult = await _client.RefreshTokenAsync(refreshToken);

            _credentials = refreshTokenResult.ToCredentials();
        }
    }


    //This is instead of loginResult
    public class Credentials
    {
        public string AccessToken { get; set; } = "";
        public string IdentityToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime AccessTokenExpiration { get; set; }
    }


    //extensions just for convenience
    public static class CredentialsExtensions
    {
        public static Credentials ToCredentials(this LoginResult loginResult) => new Credentials()
        {
            AccessToken = loginResult.AccessToken,
            IdentityToken = loginResult.IdentityToken,
            RefreshToken = loginResult.RefreshToken,
            AccessTokenExpiration = loginResult.AccessTokenExpiration
        };

        public static Credentials ToCredentials(this RefreshTokenResult refreshTokenResult) => new Credentials()
        {
            AccessToken = refreshTokenResult.AccessToken,
            IdentityToken = refreshTokenResult.IdentityToken,
            RefreshToken = refreshTokenResult.RefreshToken,
            AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
        };
    }
}
