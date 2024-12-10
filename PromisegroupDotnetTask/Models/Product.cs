using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromisegroupDotnetTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public static List<Product> GetAvailableProducts() => new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 2500m },
            new Product { Id = 2, Name = "Klawiatura", Price = 120m },
            new Product { Id = 3, Name = "Mysz", Price = 90m },
            new Product { Id = 4, Name = "Monitor", Price = 1000m },
            new Product { Id = 5, Name = "Kaczka debuggująca", Price = 66m }
        };
    }
}
