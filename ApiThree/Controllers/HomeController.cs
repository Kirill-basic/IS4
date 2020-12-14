using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiThree.Controllers
{
    public class HomeController : Controller
    {
        [Route("/secret")]
        [Authorize]
        public string Index()
        {
            var claims = User.Claims.ToList();
            return "secret from ApiThree";
        }
    }
}
