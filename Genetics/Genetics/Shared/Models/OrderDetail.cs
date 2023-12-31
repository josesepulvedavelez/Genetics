﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Shared.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [Required]
        public int OrderDetailId { get; set; }

        [Required]
        public int Amount { get; set; }

        public decimal Discount { get; set; }
        public decimal Freight { get; set; }
        public decimal Subtotal { get; set; }

        public int? AnimalId { get; set; }
        public Animal? Animal { get; set; }

        public int? OrderId { get; set;}
        public Order? Order { get; set; }
    }
}
