using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IS4
{
    public class CustomUser : IdentityUser
    {
        public CustomUser(string userName)
            :base(userName)
        {

        }
        public string Pic { get; set; }

        public byte[] Picture { get; set; }
    }
}
