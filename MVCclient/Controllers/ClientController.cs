using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Constants;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVCClient.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient _httpClient;

        public ClientController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            InitializeClient();
        }

        private void InitializeClient()
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri("https://localhost:8001")};
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IActionResult> GetSecret()
        {
            await AddTokenAsync();
            var token = await HttpContext.GetTokenAsync("access_token");

            Console.WriteLine(token);
            try
            {
                var result = await _httpClient.GetAsync("/secret");
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = result;
                }
                else
                {
                    throw new Exception("Unsuccessful status code");
                }

                return View();
            }
            catch (Exception)
            {
                await RefreshTokenAsync();
                await AddTokenAsync();

                var response = await _httpClient.GetAsync("/secret");
                Console.Write(response);
                ViewBag.Message = await response.Content.ReadAsStringAsync();
                return View();
            }
        }


        #region ExtractToService

        private async Task AddTokenAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _httpClient.SetBearerToken(accessToken);
            
            Console.WriteLine(new string('*',15));
            Console.WriteLine(accessToken);

            // _httpClient.DefaultRequestHeaders.Add("Bearer", accessToken);
        }


        private async Task RefreshTokenAsync()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshClient = _clientFactory.CreateClient();

            var resultRefreshToken = await refreshClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                ClientId = Clients.Mvc,
                ClientSecret = Secrets.MvcSecret,
                RefreshToken = refreshToken,
                Scope = "offline_access ApiOne ApiTwo ApiThree openid profile"
            });

            var authenticatingResult = await HttpContext.AuthenticateAsync("Cookie");
            authenticatingResult.Properties.UpdateTokenValue("access_token", resultRefreshToken.AccessToken);
            authenticatingResult.Properties.UpdateTokenValue("refresh_token", resultRefreshToken.RefreshToken);

            await HttpContext.SignInAsync(authenticatingResult.Principal, authenticatingResult.Properties);
        }

        #endregion
    }
}