using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameBling.ViewModels
{
    public class CardViewModel
    {
        [Required, DataType(DataType.CreditCard)]
        public int CardNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public int CVC { get; set; }

        [Required, MinLength(3)]
        public string CardHolderName { get; set; }

    }
}
