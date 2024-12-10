using PromisegroupDotnetTask.Models;

namespace PromisegroupDotnetTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var products = Product.GetAvailableProducts();
            var order = new Order();

            while(true)
            {
                Console.WriteLine("\n1. Dodaj produkt\n2. " +
                    "Usuń produkt\n3." +
                    " Wyświetl wartość zamówienia\n4. Wyjdź");
                Console.Write("Wybierz opcję: ");


                var input = Console.ReadLine();

                switch(input)
                {
                    case "1":
                        Console.WriteLine("Dostępne produkty:");
                        foreach (var product in products)
                            Console.WriteLine($"{product.Id}. {product.Name} - {product.Price} PLN");

                        Console.Write("Wybierz ID produktu: ");
                        if(int.TryParse(Console.ReadLine(), out int productId) && products.Any(p => p.Id == productId))
                        {
                            var product = products.First(p => p.Id == productId);
                            Console.Write("Podaj ilość: ");

                            if(int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                            {
                                order.AddProduct(product, quantity);
                                Console.WriteLine($"Dodano {quantity}x {product.Name}.");
                            }
                            else
                            {
                                Console.WriteLine("Niepoprawna ilość.");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Niepoprawne ID produktu.");
                        }
                        break;


                    case "2":
                        Console.Write("Podaj ID produktu do usunięcia: ");
                        if (int.TryParse(Console.ReadLine(), out productId))
                        {
                            order.RemoveProduct(productId);
                            Console.WriteLine("Produkt usunięty.");
                        }
                        else
                            Console.WriteLine("Niepoprawne ID produktu.");
                        break;

                    case "3":
                        Console.WriteLine($"Wartość zamówienia: {order.CalculateTotal():C} PLN");
                        break;

                    case "4":
                        Console.WriteLine("Do widzenia!");
                        return;

                    default:
                        Console.WriteLine("Niepoprawna opcja.");
                        break;
                }

            }
        }
    }
}