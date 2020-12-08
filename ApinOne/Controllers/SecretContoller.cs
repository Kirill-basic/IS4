using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApinOne.Controllers
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
