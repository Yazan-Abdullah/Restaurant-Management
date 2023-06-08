using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
