using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;

namespace Presentation.Options
{
    using Domain.Services;

    public class TemperatureOption
    {
        private readonly IRegulatorService _regulatorService;

        public TemperatureOption(IRegulatorService regulatorService)
        {
            _regulatorService = regulatorService;
        }

        public void Execute()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║    KONFIGURACIJA CILJNIH TEMPERATURA   ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.ResetColor();

            double dnevna = 0;
            double nocna = 0;
            bool validanUnos = false;

            while (!validanUnos)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n▶ Unesite željenu dnevnu temperaturu (°C): ");
                    Console.ResetColor();
                    dnevna = double.Parse(Console.ReadLine().Replace(',', '.'));

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("▶ Unesite željenu noćnu temperaturu (°C): ");
                    Console.ResetColor();
                    nocna = double.Parse(Console.ReadLine().Replace(',', '.'));

                    if (dnevna >= 10 && dnevna <= 30 && nocna >= 10 && nocna <= 30)
                    {
                        validanUnos = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✓ Dnevna temperatura: {dnevna}°C");
                        Console.WriteLine($"✓ Noćna temperatura: {nocna}°C");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("✗ Temperature moraju biti između 10°C i 30°C!");
                        Console.ResetColor();
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ Greška! Molimo unesite validan broj.");
                    Console.ResetColor();
                }
            }

            // Zadržava postojeći režim rada
            _regulatorService.ConfigureWorkMode(
                _regulatorService.GetCurrentMode() == Domain.Enums.RezimRada.Dnevni ? 6 : 22,
                _regulatorService.GetCurrentMode() == Domain.Enums.RezimRada.Dnevni ? 22 : 6,
                dnevna,
                nocna
            );

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Temperature uspešno podešene!");
            Console.ResetColor();

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }
    }
}