using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace sales_system
{
    class Program
    {
        private static string productFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "produkty.json");

        private static string customerFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "klienci.json");

        private static string salesFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "sprzedaze.json");

        private static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("===== Menu Sprzedaży =====");
                Console.WriteLine("1. Zarządzaj produktami");
                Console.WriteLine("2. Zarządzaj klientami");
                Console.WriteLine("3. Utwórz nową sprzedaż");
                Console.WriteLine("4. Wyświetl raporty sprzedaży");
                Console.WriteLine("5. Zrealizuj płatność");
                Console.WriteLine("6. Wyjdź");
                Console.Write("Wybierz opcję: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageProducts();
                        break;
                    case "2":
                        ManageCustomers();
                        break;
                    case "3":
                        CreateNewSale();
                        break;
                    case "4":
                        DisplaySalesReports();
                        break;
                    case "5":
                        ProcessPayment();
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Zakończenie programu... Dziękujemy za korzystanie z Systemu Sprzedaży!");
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ManageProducts()
        {
            bool returnToMenu = false;

            while (!returnToMenu)
            {
                Console.Clear();
                Console.WriteLine("===== Zarządzanie Produktami =====");
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Wyświetl listę produktów");
                Console.WriteLine("3. Usuń produkt");
                Console.WriteLine("4. Powrót do głównego menu");
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
                        returnToMenu = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddProduct()
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

        static void DisplayProducts()
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

        static void RemoveProduct()
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

            Console.WriteLine("\nAby wrócić do menu, naciśnij '0'.");
            Console.Write("\nPodaj ID produktu do usunięcia: ");
            string? idInput = Console.ReadLine();

            if (idInput == "0")
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

        static void ManageCustomers()
        {
            bool returnToMenu = false;

            while (!returnToMenu)
            {
                Console.Clear();
                Console.WriteLine("===== Zarządzanie Klientami =====");
                Console.WriteLine("1. Dodaj klienta");
                Console.WriteLine("2. Wyświetl listę klientów");
                Console.WriteLine("3. Usuń klienta");
                Console.WriteLine("4. Powrót do głównego menu");
                Console.Write("Wybierz opcję: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        DisplayCustomers();
                        break;
                    case "3":
                        RemoveCustomer();
                        break;
                    case "4":
                        returnToMenu = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Dodawanie Klienta ===");
            Console.Write("Imię klienta: ");
            string? firstName = Console.ReadLine();
            Console.Write("Nazwisko klienta: ");
            string? lastName = Console.ReadLine();
            Console.Write("Numer telefonu: ");
            string? phoneNumber = Console.ReadLine();
            Console.Write("Adres e-mail: ");
            string? email = Console.ReadLine();
            Console.Write("Adres klienta: ");
            string? address = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Nieprawidłowe dane, spróbuj ponownie.");
                Console.ReadKey();
                return;
            }

            List<Customer> customers = LoadCustomers();
            int newCustomerId = customers.Count > 0 ? customers.Max(c => c.Id) + 1 : 1;
            customers.Add(new Customer
            {
                Id = newCustomerId, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber,
                Email = email, Address = address
            });
            SaveCustomers(customers);

            Console.WriteLine("Klient został dodany.");
            Console.ReadKey();
        }

        static void DisplayCustomers()
        {
            Console.Clear();
            Console.WriteLine("=== Lista Klientów ===");
            List<Customer> customers = LoadCustomers();

            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", "ID", "Imię", "Nazwisko", "Telefon",
                "E-mail", "Adres");
            Console.WriteLine(new string('-', 100));
            foreach (var customer in customers)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", customer.Id, customer.FirstName,
                    customer.LastName, customer.PhoneNumber, customer.Email, customer.Address);
            }

            Console.WriteLine();
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu.");
            Console.ReadKey();
        }

        static void RemoveCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Usuwanie Klienta ===");

            List<Customer> customers = LoadCustomers();
            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", "ID", "Imię", "Nazwisko", "Telefon",
                "E-mail", "Adres");
            Console.WriteLine(new string('-', 100));
            foreach (var customer in customers)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", customer.Id, customer.FirstName,
                    customer.LastName, customer.PhoneNumber, customer.Email, customer.Address);
            }

            Console.WriteLine("\nAby wrócić do menu, naciśnij '0'.");
            Console.Write("\nPodaj ID klienta do usunięcia: ");
            string? idInput = Console.ReadLine();

            if (idInput == "0")
            {
                return;
            }

            if (!int.TryParse(idInput, out int customerId))
            {
                Console.WriteLine("Nieprawidłowe ID klienta.");
                Console.ReadKey();
                return;
            }

            int removed = customers.RemoveAll(c => c.Id == customerId);
            SaveCustomers(customers);

            Console.WriteLine(removed > 0 ? "Klient został usunięty." : "Nie znaleziono klienta o podanym ID.");
            Console.ReadKey();
        }

        static void CreateNewSale()
        {
            Console.Clear();
            Console.WriteLine("=== Tworzenie nowej sprzedaży ===");
            List<Product> products = LoadProducts();
            List<Customer> customers = LoadCustomers();

            Console.WriteLine("Wybierz klienta:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id}. {customer.FirstName} {customer.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine("Aby wrócić do menu, naciśnij '0'.");
            Console.WriteLine("Podaj id klienta: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            Customer selectedCustomer = customers.FirstOrDefault(c => c.Id == customerId);

            if (selectedCustomer == null)
            {
                Console.WriteLine("Nieprawidłowy wybór klienta.");
                return;
            }

            List<SaleItem> saleItems = new List<SaleItem>();
            bool addingItems = true;

            while (addingItems)
            {
                Console.Clear();
                Console.WriteLine("Wybierz produkt do sprzedaży:");
                foreach (var product in products)
                {
                    Console.WriteLine(
                        $"{product.Id}. {product.Name} - {product.Price:C} (Stan magazynowy: {product.Stock})");
                }

                Console.WriteLine();
                Console.WriteLine("Podaj id produktu: ");
                int productId = Convert.ToInt32(Console.ReadLine());
                Product selectedProduct = products.FirstOrDefault(p => p.Id == productId);

                if (selectedProduct != null && selectedProduct.Stock > 0)
                {
                    Console.Write("Ile sztuk chcesz sprzedać? ");
                    int quantity = Convert.ToInt32(Console.ReadLine());

                    if (quantity <= selectedProduct.Stock)
                    {
                        saleItems.Add(new SaleItem
                        {
                            ProductId = selectedProduct.Id,
                            ProductName = selectedProduct.Name,
                            UnitPrice = selectedProduct.Price,
                            Quantity = quantity
                        });

                        selectedProduct.Stock -= quantity;
                        SaveProducts(products);
                        Console.WriteLine("Produkt został dodany do sprzedaży.");
                    }
                    else
                    {
                        Console.WriteLine("Brak wystarczającej ilości w magazynie.");
                    }
                }

                Console.WriteLine("Czy chcesz dodać kolejny produkt? (t/n)");
                addingItems = Console.ReadLine()?.ToLower() == "t";
            }

            decimal totalAmount = saleItems.Sum(si => si.UnitPrice * si.Quantity);
            int newSaleId = LoadSales().Count + 1;
            var sale = new Sale
            {
                Id = newSaleId,
                CustomerId = selectedCustomer.Id,
                CustomerName = selectedCustomer.FirstName + " " + selectedCustomer.LastName,
                Items = saleItems,
                TotalAmount = totalAmount,
                IsPaid = false,
                Date = DateTime.Now
            };
            List<Sale> sales = LoadSales();
            sales.Add(sale);
            SaveSales(sales);

            Console.WriteLine($"Sprzedaż została zarejestrowana. Łączna kwota: {totalAmount:C}");
            Console.ReadKey();
        }

        static List<Product> LoadProducts()
        {
            if (File.Exists(productFile))
            {
                string json = File.ReadAllText(productFile);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }

            return new List<Product>();
        }

        static void SaveProducts(List<Product> products)
        {
            string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(productFile, json);
        }

        static List<Customer> LoadCustomers()
        {
            if (File.Exists(customerFile))
            {
                string json = File.ReadAllText(customerFile);
                return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
            }

            return new List<Customer>();
        }

        static void SaveCustomers(List<Customer> customers)
        {
            string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(customerFile, json);
        }

        static List<Sale> LoadSales()
        {
            if (File.Exists(salesFile))
            {
                string json = File.ReadAllText(salesFile);
                return JsonSerializer.Deserialize<List<Sale>>(json) ?? new List<Sale>();
            }

            return new List<Sale>();
        }

        static void SaveSales(List<Sale> sales)
        {
            string json = JsonSerializer.Serialize(sales, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(salesFile, json);
        }

        static void DisplaySalesReports()
        {
            Console.Clear();
            Console.WriteLine("=== Raporty sprzedaży ===");
            List<Sale> sales = LoadSales();

            if (sales.Count == 0)
            {
                Console.WriteLine("Brak danych o sprzedaży.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("{0,-10} {1,-25} {2,-15} {3,-15} {4,-15}", "Sprzedaż ID", "Klient", "Data", "Kwota", "Status płatności");
            Console.WriteLine(new string('-', 80));

            foreach (var sale in sales)
            {
                string saleDate = sale.Date.ToString("dd.MM.yyyy");
                string paymentStatus = sale.IsPaid ? "Opłacona" : "Nieopłacona";

                Console.WriteLine("{0,-10} {1,-25} {2,-15} {3,-15} {4,-15}",
                    sale.Id,
                    sale.CustomerName,
                    saleDate,
                    sale.TotalAmount.ToString("C"),
                    paymentStatus);
            }

            Console.WriteLine();
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu.");
            Console.ReadKey();
        }

        static void ProcessPayment()
        {
            Console.Clear();
            Console.WriteLine("=== Realizacja płatności ===");
            List<Sale> sales = LoadSales();

            var unpaidSales = sales.Where(s => !s.IsPaid).ToList();

            if (!unpaidSales.Any())
            {
                Console.WriteLine("Nie ma żadnych sprzedaży do opłacenia.");
                Console.WriteLine();
                Console.WriteLine("Naciśnij Enter, aby powrócić do menu.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Wybierz sprzedaż do opłacenia:");
            foreach (var sale in unpaidSales)
            {
                Console.WriteLine($"{sale.Id}. Klient: {sale.CustomerName}, Kwota: {sale.TotalAmount:C}");
            }

            Console.Write("Podaj ID sprzedaży do opłacenia lub naciśnij Enter, aby wrócić: ");
            string? saleIdInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(saleIdInput))
            {
                return;
            }

            if (!int.TryParse(saleIdInput, out int saleId))
            {
                Console.WriteLine("Nieprawidłowy ID sprzedaży.");
                Console.ReadKey();
                return;
            }

            Sale saleToPay = unpaidSales.FirstOrDefault(s => s.Id == saleId);

            if (saleToPay != null)
            {
                saleToPay.IsPaid = true;
                SaveSales(sales);
                Console.WriteLine("Płatność została zrealizowana.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono sprzedaży do opłacenia.");
            }

            Console.ReadKey();
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string? Category { get; set; }
            public string? Description { get; set; }
            public int Stock { get; set; }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
            public string? Address { get; set; }
        }

        public class SaleItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

        public class Sale
        {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public List<SaleItem> Items { get; set; } = new();
            public decimal TotalAmount { get; set; }
            public bool IsPaid { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
