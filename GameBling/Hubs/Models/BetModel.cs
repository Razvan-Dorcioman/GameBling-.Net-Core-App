using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.Hubs.Models
{
    public class BetModel
    {
        public string Username { get; set; } // unique identifier
        public int Value { get; set; } // 0 or 1 for the token
        public int Amount { get; set; } // how much credit
    }
}
