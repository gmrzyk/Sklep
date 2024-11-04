
class Program
{
    private static void Main(string[] args)
    {
        bool wyjscie = false;

        while (!wyjscie)
        {
            Console.Clear();
            Console.WriteLine("===== Menu Sprzedaży =====");
            Console.WriteLine("1. Zarządzaj produktami");
            Console.WriteLine("2. Zarządzaj klientami");
            Console.WriteLine("3. Utwórz nową sprzedaż");
            Console.WriteLine("4. Wyświetl raporty sprzedaży");
            Console.WriteLine("5. Wyjdź");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    ZarzadzajProduktami();
                    break;
                case "2":
                    ZarzadzajKlientami();
                    break;
                case "3":
                    UtworzNowaSprzedaz();
                    break;
                case "4":
                    WyswietlRaportySprzedazy();
                    break;
                case "5":
                    wyjscie = true;
                    Console.WriteLine("Zakończenie programu... Dziękujemy za korzystanie z Systemu Sprzedaży!");
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                    break;
            }
        }
    }

    static void ZarzadzajProduktami()
    {
        bool powrotDoMenu = false;

        while (!powrotDoMenu)
        {
            Console.Clear();
            Console.WriteLine("===== Zarządzanie Produktami =====");
            Console.WriteLine("1. Dodaj produkt");
            Console.WriteLine("2. Wyświetl listę produktów");
            Console.WriteLine("3. Usuń produkt");
            Console.WriteLine("4. Powrót do głównego menu");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajProdukt();
                    break;
                case "2":
                    WyswietlProdukty();
                    break;
                case "3":
                    UsunProdukt();
                    break;
                case "4":
                    powrotDoMenu = true;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                    break;
            }
        }
    }

    static void DodajProdukt()
    {
        Console.WriteLine("=== Dodawanie Produktu ===");
    }

    static void WyswietlProdukty()
    {
        Console.WriteLine("=== Lista Produktów ===");
    }

    static void UsunProdukt()
    {
        Console.WriteLine("=== Usuwanie Produktu ===");
    }

    static void ZarzadzajKlientami()
    {
        bool powrotDoMenu = false;

        while (!powrotDoMenu)
        {
            Console.Clear();
            Console.WriteLine("===== Zarządzanie Klientami =====");
            Console.WriteLine("1. Dodaj klienta");
            Console.WriteLine("2. Wyświetl listę klientów");
            Console.WriteLine("3. Usuń klienta");
            Console.WriteLine("4. Powrót do głównego menu");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajKlienta();
                    break;
                case "2":
                    WyswietlKlientow();
                    break;
                case "3":
                    UsunKlienta();
                    break;
                case "4":
                    powrotDoMenu = true;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                    break;
            }
        }
    }

    static void DodajKlienta()
    {
        Console.WriteLine("=== Dodawanie Klienta ===");
    }

    static void WyswietlKlientow()
    {
        Console.WriteLine("=== Lista Klientów ===");
    }

    static void UsunKlienta()
    {
        Console.WriteLine("=== Usuwanie Klienta ===");
    }

    static void UtworzNowaSprzedaz()
    {
        Console.WriteLine("=== Tworzenie Nowej Sprzedaży ===");
    }

    static void WyswietlRaportySprzedazy()
    {
        Console.WriteLine("=== Raporty Sprzedaży ===");
    }
}
