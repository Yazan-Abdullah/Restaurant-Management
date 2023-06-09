using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }

        [Required]
        public string TableNumber { get; set; }

        public Table()
        {
            TableNumber = $"{TableId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }
    }
}
