using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.ViewModels
{
    public class CardViewModel
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string ExpirationDate { get; set; }

        [Required]
        public string CVC { get; set; }

        [Required, MinLength(6)]
        public string CardHolderName { get; set; }

        [Required]
        public string Username { get; set; }

    }
}
