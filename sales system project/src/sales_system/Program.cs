using sales_system.Interfaces;
using sales_system.Services;

namespace sales_system
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string productFile = Path.Combine(baseDirectory, "..", "..", "..", "Data", "produkty.json");
            string customerFile = Path.Combine(baseDirectory, "..", "..", "..", "Data", "klienci.json");
            string salesFile = Path.Combine(baseDirectory, "..", "..", "..", "Data", "sprzedaze.json");
            
            IProductService productService = new ProductService(productFile);
            ICustomerService customerService = new CustomerService(customerFile);
            ISaleService saleService = new SaleService(customerService, productService, salesFile);
            
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
                Console.WriteLine("6. Wyświetl historię zakupów klienta");
                Console.WriteLine("7. Wyświetl historię sprzedaży w zakresie dat");
                Console.WriteLine("8. Wyjdź");
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
                        saleService.ShowCustomerPurchaseHistory();
                        break;
                    case "7" :
                        saleService.ShowSalesInDateRange();
                        break;
                    case "8":
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
