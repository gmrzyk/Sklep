using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    private static string productFile = "produkty.json";
    private static string customerFile = "klienci.json";

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
            Console.WriteLine("5. Zrealizuj platność");
            Console.WriteLine("6. Wyjdź");
            Console.Write("Wybierz opcję: ");

            string choice = Console.ReadLine();

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

            string choice = Console.ReadLine();

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
        Console.WriteLine("=== Dodawanie Produktu ===");
        Console.Write("Nazwa produktu: ");
        string name = Console.ReadLine();
        Console.Write("Cena produktu: ");
        decimal price = decimal.Parse(Console.ReadLine());

        List<Product> products = LoadProducts();
        products.Add(new Product { Name = name, Price = price });
        SaveProducts(products);

        Console.WriteLine("Produkt został dodany.");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void DisplayProducts()
    {
        Console.WriteLine("=== Lista Produktów ===");
        List<Product> products = LoadProducts();

        foreach (var product in products)
        {
            Console.WriteLine($"- {product.Name}: {product.Price:C}");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void RemoveProduct()
    {
        Console.WriteLine("=== Usuwanie Produktu ===");
        Console.Write("Podaj nazwę produktu do usunięcia: ");
        string name = Console.ReadLine();

        List<Product> products = LoadProducts();
        products.RemoveAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        SaveProducts(products);

        Console.WriteLine("Produkt został usunięty.");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
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

            string choice = Console.ReadLine();

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
        Console.WriteLine("=== Dodawanie Klienta ===");
        Console.Write("Imię klienta: ");
        string firstName = Console.ReadLine();
        Console.Write("Nazwisko klienta: ");
        string lastName = Console.ReadLine();

        List<Customer> customers = LoadCustomers();
        customers.Add(new Customer { FirstName = firstName, LastName = lastName });
        SaveCustomers(customers);

        Console.WriteLine("Klient został dodany.");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void DisplayCustomers()
    {
        Console.WriteLine("=== Lista Klientów ===");
        List<Customer> customers = LoadCustomers();

        foreach (var customer in customers)
        {
            Console.WriteLine($"- {customer.FirstName} {customer.LastName}");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void RemoveCustomer()
    {
        Console.WriteLine("=== Usuwanie Klienta ===");
        Console.Write("Podaj imię klienta do usunięcia: ");
        string firstName = Console.ReadLine();

        List<Customer> customers = LoadCustomers();
        customers.RemoveAll(c => c.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));
        SaveCustomers(customers);

        Console.WriteLine("Klient został usunięty.");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void CreateNewSale()
    {
        Console.WriteLine("=== Tworzenie Nowej Sprzedaży ===");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void DisplaySalesReports()
    {
        Console.WriteLine("=== Raporty Sprzedaży ===");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void ProcessPayment()
    {
        Console.WriteLine("=== Dodaj Płatność ===");
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    static List<Product> LoadProducts()
    {
        if (File.Exists(productFile))
            return JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(productFile));
        return new List<Product>();
    }

    static void SaveProducts(List<Product> products)
    {
        File.WriteAllText(productFile, JsonSerializer.Serialize(products));
    }

    static List<Customer> LoadCustomers()
    {
        if (File.Exists(customerFile))
            return JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(customerFile));
        return new List<Customer>();
    }

    static void SaveCustomers(List<Customer> customers)
    {
        File.WriteAllText(customerFile, JsonSerializer.Serialize(customers));
    }
}
