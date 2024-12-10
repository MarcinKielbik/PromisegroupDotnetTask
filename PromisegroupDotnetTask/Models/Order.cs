using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromisegroupDotnetTask.Models
{
    public class Order
    {
        public List<OrderItem> OrderItems { get; set; } = new();

        public void AddProduct(Product product, int quantity)
        {
            var existingItem = OrderItems.FirstOrDefault(o => o.Product.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                OrderItems.Add(new OrderItem { Product = product, Quantity = quantity });
            }

        }

        public void RemoveProduct(int productId)
        {
            var item = OrderItems.FirstOrDefault(o => o.Product.Id == productId);

            if (item != null)
            {
                OrderItems.Remove(item);
            }
        }

        public decimal CalculateTotal()
        {
            decimal total = OrderItems.Sum(o => o.TotalPrice);

            var sortedItems = OrderItems.OrderBy(o => o.Product.Price).ToList();
            if(sortedItems.Count >= 2)
            {
                total -= sortedItems[1].Product.Price * 0.1m;
            }

            if(sortedItems.Count >= 3)
            {
                total -= sortedItems[2].Product.Price * 0.2m;
            }

            if (total > 5000m)
                total *= 0.95m;

            return total;
        }
    }
}
