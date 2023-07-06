using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Shared.Models
{
    public class OrderModel
    {
        public Order Order { get; set; } 
        public List<OrderDetail> lstOrderDetail { get; set; }
    }
}
