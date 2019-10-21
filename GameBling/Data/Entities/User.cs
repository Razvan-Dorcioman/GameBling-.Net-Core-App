using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.Data.Entities
{
    public class User : IdentityUser
    {
        public bool Bot { get; set; }
        public bool Admin { get; set; }
        public int Balance { get; set; }
        public string Referal { get; set; }
    }
}
