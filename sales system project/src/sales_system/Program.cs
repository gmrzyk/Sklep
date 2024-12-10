using sales_system.Services;

namespace sales_system
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string productFile = Path.Combine(baseDirectory, "..", "..", "..", "produkty.json");
            string customerFile = Path.Combine(baseDirectory, "..", "..", "..", "klienci.json");
            string salesFile = Path.Combine(baseDirectory, "..", "..", "..", "sprzedaze.json");

            var productService = new ProductService(productFile);
            var customerService = new CustomerService(customerFile);
            var saleService = new SaleService(customerService, productService, salesFile);

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
                        productService.ManageProducts();
                        break;
                    case "2":
                        customerService.ManageCustomers();
                        break;
                    case "3":
                        saleService.CreateNewSale();
                        break;
                    case "4":
                        saleService.DisplaySalesReports();
                        break;
                    case "5":
                        saleService.ProcessPayment();
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Zakończenie programu...");
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
            }
        }
    }
}
