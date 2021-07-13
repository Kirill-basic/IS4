using IdentityModel.OidcClient;
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
        private LoginResult _loginResult;
        public ClaimsPrincipal User;
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
                ClientId = Constants.Clients.Wpf,
                Scope = "openid profile",
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
            _loginResult = await _client.LoginAsync();
            User = _loginResult.User;
        }

        public async Task GetRequestAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loginResult.AccessToken);
            var apiResult = await client.GetStringAsync("https://localhost:44387/secret");
        }
    }
}
