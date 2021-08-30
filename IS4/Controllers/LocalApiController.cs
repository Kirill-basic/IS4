using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IS4.Controllers
{
    [Route("localApi")]
    //[Authorize]
    [Authorize(LocalApi.PolicyName)]
    public class LocalApiController : Controller
    {
        public LocalApiController()
        {
            System.Console.WriteLine("Inited localaapi");

        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Fuck U");
        }
    }
}
