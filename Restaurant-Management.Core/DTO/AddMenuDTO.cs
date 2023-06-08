using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.DTO
{
    public class AddMenuDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }
    }
}
