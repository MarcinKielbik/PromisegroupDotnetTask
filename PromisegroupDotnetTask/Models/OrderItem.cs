using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromisegroupDotnetTask.Models
{
    public class OrderItem
    {
        public required Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Product.Price * Quantity;
    }
}
