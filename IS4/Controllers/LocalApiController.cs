using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IS4.Controllers
{
    [Route("localApi")]
    [Authorize(LocalApi.PolicyName)]
    public class LocalApiController : Controller
    {
        public IActionResult Get()
        {
            var user = User;

            return View();
        }
    }
}
