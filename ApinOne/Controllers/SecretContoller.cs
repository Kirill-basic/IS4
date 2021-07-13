using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ApiOne.Controllers
{
    public class SecretContoller : Controller
    {
        [Route("/secret")]
        [Authorize]
        public string Index()
        {
            var claims = User.Claims.ToList();

            var claim = User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
            var resourceList = User.Identities.ToList();
            var resource = resourceList.First();
            if (!User.IsInRole("Admin"))
            {
                return "this is admin";
            }

            User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Country, "Russia") }));
            return "Secret message from apione";
        }
    }
}
