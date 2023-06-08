using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        
        public int? MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
        public int? Quantity { get; set; }
    }
}
