using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
