using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;

namespace NoSecretMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _httpClientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public IActionResult TestClaim()
        {
            var claims = User.Claims.ToList();
            var identity = User.Identity;
            var accessToken = HttpContext.GetTokenAsync("access_token");

            return View();
        }


        [Authorize]
        public async Task<IActionResult> Test()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var result = await GetSecretAsync(accessToken);

                ViewBag.Message = result;
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<string> GetSecretAsync(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);

            var newResult = await apiClient.GetAsync("https://localhost:8001/secret");
            var result = await apiClient.GetStringAsync("https://localhost:8001/secret");

            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentNullException();
            }

            return result;
        }
    }
}