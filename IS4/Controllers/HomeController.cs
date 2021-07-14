using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Constants;

namespace IS4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient _httpClient;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _httpClient = new HttpClient() {BaseAddress = new Uri("https://localhost:8001")};
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _clientFactory = clientFactory;
        }


        public IActionResult Index()
        {
            var user = User;

            var claims = user.Claims;

            return View();
        }

        
        public IActionResult Secret1Async()
        {
            var claims = User.Claims;
            return View();
        }

        
        public async Task<IActionResult> Secret2Async()
        {
            await AddTokenAsync();

            var response = await _httpClient.GetAsync("/secret");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = await response.Content.ReadAsStringAsync();
                return View();
            }

            await RefreshTokenAsync();
            await AddTokenAsync();

            var updatedResponse = await _httpClient.GetAsync("/secret");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = await updatedResponse.Content.ReadAsStringAsync();
            }
            else
            {
                ViewBag.Message = "Ooops";
            }

            return View();
        }


        #region ExtractToService
        private async Task AddTokenAsync()
        {
            var context = HttpContext;
            if (context is null)
            {
                Console.WriteLine("Context is null");
            }
            
            var list = context?.Request.Headers.ToList();
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _httpClient.SetBearerToken(accessToken);

            Console.WriteLine(new string('*', 15));
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