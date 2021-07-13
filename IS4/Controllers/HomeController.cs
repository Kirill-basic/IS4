using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static IdentityServer4.IdentityServerConstants;

namespace IS4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = User;

            var claims = user.Claims;

            return View();
        }

        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Secret1Async()
        {
            var claims = User.Claims;
            return View();
        }

        [Authorize]
        public IActionResult Secret2()
        {
            var claims = User.Claims;
            return View();
        }
    }
}
