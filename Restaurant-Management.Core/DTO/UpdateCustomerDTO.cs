using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.DTO
{
    public class UpdateCustomerDTO
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? Phone { get; set; }
    }
}
