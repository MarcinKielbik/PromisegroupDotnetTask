using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromisegroupDotnetTask.Models
{
    public class OrderService
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
            // Initial sum
            decimal total = OrderItems.Sum(item => item.Product.Price * item.Quantity);

            // Applying discounts:
            // Discount for orders with 3 or more products
            if (OrderItems.Count >= 3)
            {
                total *= 0.9m; // 10% discount
            }
            // Discount for orders with 2 products
            else if (OrderItems.Count == 2)
            {
                total *= 0.95m; // 5% discount
            }

            // Discount for orders over 5000 PLN
            if (total > 5000)
            {
                total *= 0.95m; // 5% discount
            }

            // Rounding to two decimal places
            return Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }




        public void SaveToHistory()
        {
            string filePath = "OrderHistory.txt";

            if (!OrderItems.Any())
            {
                Console.WriteLine("Brak produktów w zamówieniu. Nic nie zapisano.");
                return;
            }

            StringBuilder orderSummary = new StringBuilder();
            decimal totalBeforeDiscount = 0m;
            decimal totalAfterDiscount = CalculateTotal();

            foreach (var item in OrderItems)
            {
                decimal itemTotalPrice = item.Product.Price * item.Quantity;
                totalBeforeDiscount += itemTotalPrice;

                orderSummary.AppendLine($"{item.Product.Name} x{item.Quantity} " +
                    $"- {item.Product.Price} PLN/ szt. " +
                    $"- {itemTotalPrice} PLN (cena za pozycję)");
            }

            // Displaying discount information
            orderSummary.AppendLine($"Suma przed rabatami: {totalBeforeDiscount} PLN");

            // Sort items by price for discount purposes
            var sortedItems = OrderItems.OrderBy(o => o.Product.Price).ToList();

            if (sortedItems.Count >= 2)
            {
                decimal discount = sortedItems[1].Product.Price * 0.1m;
                orderSummary.AppendLine($"Rabaty: {discount} PLN (10% na drugi najtańszy produkt)");
                totalAfterDiscount -= discount;
            }

            if (sortedItems.Count >= 3)
            {
                decimal discount = sortedItems[2].Product.Price * 0.2m;
                orderSummary.AppendLine($"Rabaty: {discount} PLN (20% na trzeci najtańszy produkt)");
                totalAfterDiscount -= discount;
            }

            if (totalAfterDiscount > 5000m)
            {
                decimal discount = totalAfterDiscount * 0.05m;
                orderSummary.AppendLine($"Rabaty: {discount} PLN (5% rabatu przy zamówieniu powyżej 5000 PLN)");
                totalAfterDiscount -= discount;
            }

            orderSummary.AppendLine($"Całkowita wartość zamówienia po rabatach: {totalAfterDiscount} PLN");
            orderSummary.AppendLine($"Data: {DateTime.Now}");
            orderSummary.AppendLine(new string('-', 30));

            try
            {
                File.AppendAllText(filePath, orderSummary.ToString());
                Console.WriteLine("Zamówienie zapisano w historii.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisu do pliku: {ex.Message}");
            }
        }
    }


}
