using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
      
        public string? Name { get; set; }
       
        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Key { get; set; }
        public string? Iv { get; set; }
    }
}
