using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IS4.Controllers
{
    [Route("localApi")]
    [Authorize(LocalApi.PolicyName)]
    public class LocalApiController : Controller
    {
        public IActionResult Index()
        {
            var claims = User.Claims;
            throw new NotImplementedException();
        }
    }
}
