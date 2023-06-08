using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }
    }
}
