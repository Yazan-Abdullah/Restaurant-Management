using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.DTO
{
    public class UpDateOrderItem
    {
        public int? MenuId { get; set; }
        public int? Quantity { get; set; }
    }
}
