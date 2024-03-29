﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
        public string? Email { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}
