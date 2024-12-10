using System.Text.Json;
using sales_system.Models;

namespace sales_system.Services
{
    public class SaleService
    {
        private readonly string salesFile;
        private readonly CustomerService customerService;
        private readonly ProductService productService;

        public SaleService(CustomerService customerService, ProductService productService, string salesFile)
        {
            this.customerService = customerService;
            this.productService = productService;
            this.salesFile = salesFile;
        }

        public void CreateNewSale()
        {
            Console.Clear();
            Console.WriteLine("=== Tworzenie nowej sprzedaży ===");
            List<Product> products = productService.LoadProducts();
            List<Customer> customers = customerService.LoadCustomers();

            Console.WriteLine("Wybierz klienta:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id}. {customer.FirstName} {customer.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine("Podaj id klienta lub naciśnij Enter, aby wrócić: ");
            string? customerInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(customerInput))
            {
                return;
            }

            if (!int.TryParse(customerInput, out int customerId))
            {
                Console.WriteLine("Nieprawidłowy wybór klienta.");
                Console.ReadKey();
                return;
            }

            Customer selectedCustomer = customers.FirstOrDefault(c => c.Id == customerId);

            if (selectedCustomer == null)
            {
                Console.WriteLine("Nie znaleziono klienta.");
                Console.ReadKey();
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
                    Console.WriteLine($"{product.Id}. {product.Name} - {product.Price:C} (Stan magazynowy: {product.Stock})");
                }

                Console.WriteLine();
                Console.WriteLine("Podaj id produktu lub naciśnij Enter, aby zakończyć:");
                string? productInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(productInput))
                {
                    break;
                }

                if (!int.TryParse(productInput, out int productId))
                {
                    Console.WriteLine("Nieprawidłowe ID produktu.");
                    Console.ReadKey();
                    return;
                }

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
                        productService.SaveProducts(products);
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

        public void DisplaySalesReports()
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

        public void ProcessPayment()
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

            Console.WriteLine();
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

        private List<Sale> LoadSales()
        {
            if (File.Exists(salesFile))
            {
                string json = File.ReadAllText(salesFile);
                return JsonSerializer.Deserialize<List<Sale>>(json) ?? new List<Sale>();
            }
            return new List<Sale>();
        }

        private void SaveSales(List<Sale> sales)
        {
            string json = JsonSerializer.Serialize(sales, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(salesFile, json);
        }
    }
}
