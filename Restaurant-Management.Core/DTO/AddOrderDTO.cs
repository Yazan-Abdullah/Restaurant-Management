using Restaurant_Management.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.DTO
{
    public class AddOrderDTO
    {
        public int CustomerId { get; set; }

        public int TableId { get; set; }
        public string? TableNumber { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}
