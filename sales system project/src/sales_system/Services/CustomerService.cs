using System.Text.Json;
using sales_system.Models;

namespace sales_system.Services
{
    public class CustomerService
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

        private void AddCustomer()
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

        private void DisplayCustomers()
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

        private void RemoveCustomer()
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
