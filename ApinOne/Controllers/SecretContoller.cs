using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiOne.Controllers
{
    public class SecretContoller : Controller
    {
        [Route("/secret")]
        [Authorize]
        public string Index()
        {
            var claims = User.Claims.ToList();
            return "Secret message from apione";
        }
    }
}
