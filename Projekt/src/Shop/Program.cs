
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

            if (!wyjscie)
            {
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu...");
                Console.ReadKey();
            }
        }
    }


    static void ZarzadzajProduktami()
    {
        Console.WriteLine("=== Zarządzanie Produktami ===");

    }


    static void ZarzadzajKlientami()
    {
        Console.WriteLine("=== Zarządzanie Klientami ===");
        
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
