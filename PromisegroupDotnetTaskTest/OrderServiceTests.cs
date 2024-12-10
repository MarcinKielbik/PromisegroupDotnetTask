using PromisegroupDotnetTask.Models;
using Xunit;

namespace PromisegroupDotnetTaskTest
{
    public class OrderServiceTests
    {
        [Fact]
        public void AddProduct_ShouldAddNewProduct_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
            var service = new OrderService();

            // Act
            service.AddProduct(product, 1);

            // Assert
            Assert.Single(service.OrderItems);
            Assert.Equal(product, service.OrderItems[0].Product);
            Assert.Equal(1, service.OrderItems[0].Quantity);
        }

        [Fact]
        public void AddProduct_ShouldIncreaseQuantity_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
            var service = new OrderService();
            service.AddProduct(product, 1);

            // Act
            service.AddProduct(product, 2);

            // Assert
            Assert.Single(service.OrderItems);
            Assert.Equal(3, service.OrderItems[0].Quantity);
        }

        [Fact]
        public void RemoveProduct_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
            var service = new OrderService();
            service.AddProduct(product, 1);

            // Act
            service.RemoveProduct(product.Id);

            // Assert
            Assert.Empty(service.OrderItems);
        }

        [Fact]
        public void CalculateTotal_ShouldReturnZero_WhenNoProductsAdded()
        {
            // Arrange
            var service = new OrderService();

            // Act
            var total = service.CalculateTotal();

            // Assert
            Assert.Equal(0, total);
        }

        [Fact]
        public void CalculateTotal_ShouldApplyDiscountsForTwoOrThreeProducts()
        {
            // Arrange
            var service = new OrderService();
            var product1 = new Product { Id = 1, Name = "Product 1", Price = 100 };
            var product2 = new Product { Id = 2, Name = "Product 2", Price = 200 };
            var product3 = new Product { Id = 3, Name = "Product 3", Price = 300 };

            
            service.AddProduct(product1, 1);
            service.AddProduct(product2, 1);

            // Act
            var totalTwoProducts = service.CalculateTotal();

            // Assert for two products
            Assert.Equal(285.00m, totalTwoProducts);

            // add third product
            service.AddProduct(product3, 1);

            // Act
            var totalThreeProducts = service.CalculateTotal();

            // Assert for three products
            Assert.Equal(540.00m, totalThreeProducts);
        }

        [Fact]
        public void CalculateTotal_ShouldApplyDiscountForLargeOrders()
        {
            // Arrange
            var service = new OrderService();
            var product1 = new Product { Id = 1, Name = "Product 1", Price = 3000 };
            var product2 = new Product { Id = 2, Name = "Product 2", Price = 3000 };
            var product3 = new Product { Id = 3, Name = "Product 3", Price = 2000 };

            // Add products with total > 5000
            service.AddProduct(product1, 1);
            service.AddProduct(product2, 1);
            service.AddProduct(product3, 1);

            // Act
            var total = service.CalculateTotal();

            // Assert for the sum above 5000
            Assert.Equal(6840.00m, total);
        }



    }
}
