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
            decimal total = OrderItems.Sum(o => o.TotalPrice);

            var sortedItems = OrderItems.OrderBy(o => o.Product.Price).ToList();
            if (sortedItems.Count >= 2)
            {
                total -= sortedItems[1].Product.Price * 0.1m;
            }

            if (sortedItems.Count >= 3)
            {
                total -= sortedItems[2].Product.Price * 0.2m;
            }

            if (total > 5000m)
                total *= 0.95m;

            return total;
        }
        /*
        public void SaveToHistory()
        {
            string filePath = "OrderHistory.txt";

            StringBuilder orderSummary = new StringBuilder();
            orderSummary.AppendLine("Zamówienie:");
            foreach (var item in OrderItems)
            {
                orderSummary.AppendLine($"{item.Product.Name} x{item.Quantity} - {item.TotalPrice} PLN");
            }

            orderSummary.AppendLine($"Całkowita wartość zamówienia: {CalculateTotal()} PLN");
            orderSummary.AppendLine($"Data: {DateTime.Now}");
            orderSummary.AppendLine(new string('-', 30));

            File.AppendAllText(filePath, orderSummary.ToString());
        }*/

        public void SaveToHistory()
        {
            string filePath = "OrderHistory.txt";

            if (!OrderItems.Any())
            {
                Console.WriteLine("Brak produktów w zamówieniu. Nic nie zapisano.");
                return;
            }

            StringBuilder orderSummary = new StringBuilder();
            orderSummary.AppendLine("Zamówienie:");
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
