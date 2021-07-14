using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Constants;
using Newtonsoft.Json;

namespace MVCClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        private async Task RefreshToken(string refreshToken)
        {
            var refreshClient = _httpClientFactory.CreateClient();
            var resultRefreshTokenAsync = await refreshClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                ClientId = Clients.Mvc,
                ClientSecret = Secrets.MvcSecret,
                RefreshToken = refreshToken,
                Scope = "offline_access ApiOne ApiTwo ApiThree openid profile"
            });

            await UpdateAuthenticatedContext(resultRefreshTokenAsync.AccessToken, resultRefreshTokenAsync.RefreshToken);
        }

        
        [Authorize]
        public async Task<IActionResult> SecretAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            try
            {
                var result = await GetSecretAsync(accessToken);

                ViewBag.Message = result;
                return View();
            }
            catch (Exception e)
            {
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                //custom implementation of refreshing
                // await GetRefreshed(refreshToken);
                await RefreshToken(refreshToken);

                var newAccessToken = await HttpContext.GetTokenAsync("access_token");

                var result = await GetSecretAsync(newAccessToken);
                ViewBag.Message = result;
                return View();
            }
        }


        private async Task<string> GetSecretAsync(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);

            var result = await apiClient.GetStringAsync("https://localhost:8001/secret");

            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentNullException();
            }

            return result;
        }

        
        #region OldMethods

        public async Task<string> GetRefreshed(string refreshToken)
        {
            var refreshClient = _httpClientFactory.CreateClient();

            var parameters = new Dictionary<string, string>
            {
                ["refresh_token"] = refreshToken,
                ["grant_type"] = "refresh_token",
                ["client_id"] = Clients.Mvc,
                ["client_secret"] = Secrets.MvcSecret
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/connect/token")
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            var basics = "username:password";
            var encodedData = Encoding.UTF8.GetBytes(basics);
            var encoded64BaseData = Convert.ToBase64String(encodedData);

            request.Headers.Add("Authorization", $"Bearer {encoded64BaseData}");
            var refreshResponse = await refreshClient.SendAsync(request);

            var tokenData = await refreshResponse.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenData);
            var newAccessToken = tokenResponse.GetValueOrDefault("access_token");
            var newRefreshToken = tokenResponse.GetValueOrDefault("refresh_token");

            await UpdateAuthenticatedContext(newAccessToken, newRefreshToken);
            return newAccessToken;
        }


        private async Task UpdateAuthenticatedContext(string accessToken, string refreshToken)
        {
            //TODO: error here. Wrong authentication scheme
            var authenticatingResult = await HttpContext.AuthenticateAsync("Cookie");

            authenticatingResult.Properties.UpdateTokenValue("access_token", accessToken);
            authenticatingResult.Properties.UpdateTokenValue("refresh_token", refreshToken);

            await HttpContext.SignInAsync(authenticatingResult.Principal, authenticatingResult.Properties);
        }

        #endregion


        [Authorize(Policy = "IsManager")]
        public async Task<IActionResult> GetApiThreeSecret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync("https://localhost:7001/secret");
            var content = await response.Content.ReadAsStringAsync();
            ViewBag.Message = content;

            return View();
        }
    }
}