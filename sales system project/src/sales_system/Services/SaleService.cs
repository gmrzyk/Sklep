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
                Console.WriteLine($"{customer.Id}. {customer.FirstName} {customer.LastName} (Limit: {customer.UnpaidLimit:C})");
            }

            Console.WriteLine();
            Console.WriteLine("Aby wrócić do menu, naciśnij Enter.");
            Console.WriteLine("Podaj id klienta: ");
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
    
            decimal currentUnpaidAmount = LoadSales()
                .Where(s => s.CustomerId == customerId && !s.IsPaid)
                .Sum(s => s.TotalAmount);

            Console.WriteLine($"Obecna nieopłacona kwota: {currentUnpaidAmount:C}");
            Console.WriteLine($"Limit nieopłaconej kwoty: {selectedCustomer.UnpaidLimit:C}");

            if (currentUnpaidAmount >= selectedCustomer.UnpaidLimit)
            {
                Console.WriteLine("Klient osiągnął swój limit nieopłaconych kwot. Nie można dodać nowej sprzedaży.");
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

            if (currentUnpaidAmount + totalAmount > selectedCustomer.UnpaidLimit)
            {
                Console.WriteLine("Dodanie tej sprzedaży przekroczyłoby limit nieopłaconej kwoty klienta.");
             Console.ReadKey();
                return;
            }

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
        
        public void ShowCustomerPurchaseHistory()
        {
            Console.Clear();
            Console.WriteLine("=== Historia Zakupów Klienta ===");
            List<Sale> sales = LoadSales();
            List<Customer> customers = customerService.LoadCustomers();

            Console.WriteLine("Lista klientów:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id}. {customer.FirstName} {customer.LastName}");
            }

            Console.WriteLine("Podaj ID klienta, którego zakupy chcesz wyświetlić (lub naciśnij Enter, aby wrócić): ");
            string? customerInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(customerInput))
            {
                return;
            }
    
            if (!int.TryParse(customerInput, out int customerId))
            {
                Console.WriteLine("Nieprawidłowe ID klienta.");
                Console.ReadKey();
                return;
            }
    
            Customer selectedCustomer = customers.FirstOrDefault(c => c.Id == customerId);

            if (selectedCustomer == null)
            {
                Console.WriteLine("Nie znaleziono klienta o podanym ID.");
                Console.ReadKey();
                return;
            }
            
            var customerSales = sales.Where(s => s.CustomerId == customerId);

            if (!customerSales.Any())
            {
                Console.WriteLine($"Brak zakupów dla klienta {selectedCustomer.FirstName} {selectedCustomer.LastName}.");
                Console.ReadKey();
                return;
            }
            
            Console.Clear();
            Console.WriteLine($"Historia zakupów dla: {selectedCustomer.FirstName} {selectedCustomer.LastName}");
            Console.WriteLine("{0,-10} {1,-20} {2,-15} {3,-15}", "Zakup ID", "Data", "Kwota", "Status");
            Console.WriteLine(new string('-', 60));

            foreach (var sale in customerSales)
            {
                string status = sale.IsPaid ? "Opłacona" : "Nieopłacona";
                Console.WriteLine("{0,-10} {1,-20} {2,-15:C} {3,-15}", sale.Id, sale.Date.ToString("dd.MM.yyyy"), sale.TotalAmount, status);
            }
            
            decimal totalPaid = customerSales.Where(s => s.IsPaid).Sum(s => s.TotalAmount);
            decimal totalUnpaid = customerSales.Where(s => !s.IsPaid).Sum(s => s.TotalAmount);

            Console.WriteLine($"\nŁączna kwota opłacona przez klienta: {totalPaid:C}");
            Console.WriteLine($"Łączna kwota nieopłacona przez klienta: {totalUnpaid:C}");
            Console.WriteLine("Naciśnij Enter, aby wrócić do menu.");
            Console.ReadKey();
        }

        public void ShowSalesInDateRange()
        {
            Console.Clear();
            Console.WriteLine("=== Historia Sprzedaży w Zakresie Dat ===");

            List<Sale> sales = LoadSales();

            if (!sales.Any())
            {
                Console.WriteLine("Brak danych o sprzedaży.");
                Console.ReadKey();
                return;
            }

            Console.Write("Podaj datę początkową (format: RRRR-MM-DD): ");
            string? startDateInput = Console.ReadLine();

            Console.Write("Podaj datę końcową (format: RRRR-MM-DD): ");
            string? endDateInput = Console.ReadLine();

            if (!DateTime.TryParse(startDateInput, out DateTime startDate) ||
                !DateTime.TryParse(endDateInput, out DateTime endDate))
            {
                Console.WriteLine("Nieprawidłowy format daty. Spróbuj ponownie.");
                Console.ReadKey();
                return;
            }

            if (endDate < startDate)
            {
                Console.WriteLine("Data końcowa nie może być wcześniejsza niż data początkowa.");
                Console.ReadKey();
                return;
            }

            var filteredSales = sales.Where(s => s.Date >= startDate && s.Date <= endDate);

            if (!filteredSales.Any())
            {
                Console.Clear();
                Console.WriteLine($"Brak sprzedaży w zakresie {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}.");
                Console.ReadKey();
                return;
            }
            Console.Clear();
            Console.WriteLine($"Sprzedaż w zakresie {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}:");
            Console.WriteLine("{0,-10} {1,-25} {2,-15} {3,-15}", "ID", "Klient", "Data", "Kwota");
            Console.WriteLine(new string('-', 70));

            foreach (var sale in filteredSales)
            {
                Console.WriteLine("{0,-10} {1,-25} {2,-15} {3,-15:C}",
                    sale.Id,
                    sale.CustomerName,
                    sale.Date.ToString("yyyy-MM-dd"),
                    sale.TotalAmount);
            }

            decimal totalAmountInRange = filteredSales.Sum(s => s.TotalAmount);
            Console.WriteLine($"\nŁączna wartość sprzedaży w podanym okresie: {totalAmountInRange:C}");

            Console.WriteLine("\nNaciśnij Enter, aby wrócić do menu.");
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
