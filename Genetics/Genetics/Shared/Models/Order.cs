using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Shared.Models
{
    [Table("Order")]
    public  class Order
    {
        [Key]
        [Required]
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPurchase { get; set; }

        public ICollection<OrderDetail>? OrderDetail { get; set; }
    }
}
