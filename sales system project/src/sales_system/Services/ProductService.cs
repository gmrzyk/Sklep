using System.Text.Json;
using sales_system.Models;

namespace sales_system.Services
{
    public class ProductService
    {
        private readonly string productFile;

        public ProductService(string productFile)
        {
            this.productFile = productFile;
        }

        public void ManageProducts()
        {
            bool returnToMenu = false;

            while (!returnToMenu)
            {
                Console.Clear();
                Console.WriteLine("===== Zarządzanie Produktami =====");
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Wyświetl listę produktów");
                Console.WriteLine("3. Usuń produkt");
                Console.WriteLine("4. Zmień stan magazynowy");
                Console.WriteLine("5. Powrót do głównego menu");
                Console.Write("Wybierz opcję: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        DisplayProducts();
                        break;
                    case "3":
                        RemoveProduct();
                        break;
                    case "4":
                        UpdateStock();
                        break;
                    case "5":
                        returnToMenu = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Dodawanie Produktu ===");
            Console.Write("Nazwa produktu: ");
            string? name = Console.ReadLine();
            Console.Write("Cena produktu: ");
            string? priceInput = Console.ReadLine();
            Console.Write("Kategoria produktu: ");
            string? category = Console.ReadLine();
            Console.Write("Opis produktu: ");
            string? description = Console.ReadLine();
            Console.Write("Stan magazynowy: ");
            string? stockInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || !decimal.TryParse(priceInput, out decimal price) ||
                !int.TryParse(stockInput, out int stock))
            {
                Console.WriteLine("Nieprawidłowe dane, spróbuj ponownie.");
                Console.ReadKey();
                return;
            }

            List<Product> products = LoadProducts();
            int newProductId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            products.Add(new Product
            {
                Id = newProductId, Name = name, Price = price, Category = category, Description = description,
                Stock = stock
            });
            SaveProducts(products);

            Console.WriteLine("Produkt został dodany.");
            Console.ReadKey();
        }

        private void DisplayProducts()
        {
            Console.Clear();
            Console.WriteLine("=== Lista Produktów ===");
            List<Product> products = LoadProducts();

            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-30} {5,-10}", "ID", "Nazwa", "Cena", "Kategoria",
                "Opis", "Stan magazynowy");
            Console.WriteLine(new string('-', 105));
            foreach (var product in products)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15:C} {3,-15} {4,-30} {5,-10}", product.Id, product.Name,
                    product.Price, product.Category, product.Description, product.Stock);
            }

            Console.WriteLine();
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu.");
            Console.ReadKey();
        }

        private void RemoveProduct()
        {
            Console.Clear();
            Console.WriteLine("=== Usuwanie Produktu ===");

            List<Product> products = LoadProducts();
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-30} {5,-10}", "ID", "Nazwa", "Cena", "Kategoria",
                "Opis", "Stan magazynowy");
            Console.WriteLine(new string('-', 105));

            foreach (var product in products)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15:C} {3,-15} {4,-30} {5,-10}", product.Id, product.Name,
                    product.Price, product.Category, product.Description, product.Stock);
            }

            Console.WriteLine();
            Console.Write("Podaj ID produktu do usunięcia lub naciśnij Enter, aby wrócić: ");
            string? idInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idInput))
            {
                return;
            }

            if (!int.TryParse(idInput, out int productId))
            {
                Console.WriteLine("Nieprawidłowe ID produktu.");
                Console.ReadKey();
                return;
            }

            int removed = products.RemoveAll(p => p.Id == productId);
            SaveProducts(products);

            Console.WriteLine(removed > 0 ? "Produkt został usunięty." : "Nie znaleziono produktu o podanym ID.");
            Console.ReadKey();
        }

        private void UpdateStock()
        {
            Console.Clear();
            Console.WriteLine("=== Zmiana Stanu Magazynowego ===");

            List<Product> products = LoadProducts();
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-30} {5,-10}", "ID", "Nazwa", "Cena", "Kategoria",
                "Opis", "Stan magazynowy");
            Console.WriteLine(new string('-', 105));

            foreach (var product in products)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15:C} {3,-15} {4,-30} {5,-10}", product.Id, product.Name,
                    product.Price, product.Category, product.Description, product.Stock);
            }

            Console.WriteLine();
            Console.Write("Podaj ID produktu, którego stan magazynowy chcesz zmienić lub naciśnij Enter, aby wrócić: ");
            string? idInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idInput))
            {
                return;
            }

            if (!int.TryParse(idInput, out int productId))
            {
                Console.WriteLine("Nieprawidłowe ID produktu.");
                Console.ReadKey();
                return;
            }

            Product selectedProduct = products.FirstOrDefault(p => p.Id == productId);

            if (selectedProduct == null)
            {
                Console.WriteLine("Nie znaleziono produktu o podanym ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Podaj nowy stan magazynowy: ");
            string? newStockInput = Console.ReadLine();

            if (int.TryParse(newStockInput, out int newStock))
            {
                selectedProduct.Stock = newStock;
                SaveProducts(products);
                Console.WriteLine("Stan magazynowy został zaktualizowany.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowy stan magazynowy.");
            }

            Console.ReadKey();
        }

        public List<Product> LoadProducts()
        {
            if (File.Exists(productFile))
            {
                string json = File.ReadAllText(productFile);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
            return new List<Product>();
        }

        public void SaveProducts(List<Product> products)
        {
            string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(productFile, json);
        }
    }
}
