using System.Text.Json;
using sales_system.Models;
using sales_system.Interfaces;

namespace sales_system.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly string customerFile;

        public CustomerService(string customerFile)
        {
            this.customerFile = customerFile;
        }

        public void ManageCustomers()
        {
            bool returnToMenu = false;

            while (!returnToMenu)
            {
                Console.Clear();
                Console.WriteLine("===== Zarządzanie Klientami =====");
                Console.WriteLine("1. Dodaj klienta");
                Console.WriteLine("2. Wyświetl listę klientów");
                Console.WriteLine("3. Usuń klienta");
                Console.WriteLine("4. Ustaw limit na nieopłacone kwoty");
                Console.WriteLine("5. Powrót do głównego menu");
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
                        SetUnpaidLimit();
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

        public void AddCustomer()
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

        public void DisplayCustomers()
        {
            Console.Clear();
            Console.WriteLine("=== Lista Klientów ===");
            List<Customer> customers = LoadCustomers();

            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30} {6,-15}", 
                "ID", "Imię", "Nazwisko", "Telefon", "E-mail", "Adres", "Limit");
            Console.WriteLine(new string('-', 130));

            foreach (var customer in customers)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30} {6,-15:C}", 
                    customer.Id, 
                    customer.FirstName, 
                    customer.LastName, 
                    customer.PhoneNumber ?? "Brak", 
                    customer.Email ?? "Brak", 
                    customer.Address ?? "Brak", 
                    customer.UnpaidLimit);
            }

            Console.WriteLine();
            Console.WriteLine("Naciśnij Enter, aby powrócić do menu.");
            Console.ReadKey();
        }

        public void RemoveCustomer()
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

            Console.WriteLine();
            Console.Write("Podaj ID klienta do usunięcia lub naciśnij Enter, aby wrócić: ");
            string? idInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idInput))
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
        
        public void SetUnpaidLimit()
        {
            Console.Clear();
            Console.WriteLine("=== Ustawienie Limitów na Nieopłacone Kwoty ===");

            List<Customer> customers = LoadCustomers();
            Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-15}", "ID", "Imię i nazwisko", "E-mail", "Aktualny Limit");
            Console.WriteLine(new string('-', 75));

            foreach (var customer in customers)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-15:C}", 
                    customer.Id, 
                    $"{customer.FirstName} {customer.LastName}", 
                    customer.Email ?? "Brak", 
                    customer.UnpaidLimit);
            }

            Console.WriteLine();
            Console.WriteLine("Podaj ID klienta, któremu chcesz ustawić limit (lub naciśnij Enter, aby wrócić): ");
            string? idInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idInput))
            {
                return;
            }

            if (!int.TryParse(idInput, out int customerId))
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

            Console.Write($"Podaj nowy limit dla klienta {selectedCustomer.FirstName} {selectedCustomer.LastName}: ");
            string? limitInput = Console.ReadLine();

            if (!decimal.TryParse(limitInput, out decimal newLimit) || newLimit < 0)
            {
                Console.WriteLine("Nieprawidłowy limit.");
                Console.ReadKey();
                return;
            }

            selectedCustomer.UnpaidLimit = newLimit;
            SaveCustomers(customers);

            Console.WriteLine($"Nowy limit dla klienta {selectedCustomer.FirstName} {selectedCustomer.LastName}: {newLimit:C}");
            Console.ReadKey();
        }
        
        public List<Customer> LoadCustomers()
        {
            if (File.Exists(customerFile))
            {
                string json = File.ReadAllText(customerFile);
                return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
            }
            return new List<Customer>();
        }

        private void SaveCustomers(List<Customer> customers)
        {
            string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(customerFile, json);
        }
    }
}
