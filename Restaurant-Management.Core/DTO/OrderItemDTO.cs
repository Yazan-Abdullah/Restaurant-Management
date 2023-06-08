using Restaurant_Management.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.DTO
{
    public class OrderItemDTO
    {
        public int? MenuId { get; set; }
        public int? Quantity { get; set; }
    }
}
