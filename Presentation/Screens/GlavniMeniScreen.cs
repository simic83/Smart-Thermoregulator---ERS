using System;
using Infrastructure;

namespace Presentation.Screens
{
    public static class GlavniMeniScreen
    {
        public static void Prikazi(
            Application.Services.DeviceService deviceService,
            Application.Services.HeaterService heaterService,
            Application.Services.RegulatorService regulatorService,
            Infrastructure.FileLogger fileLogger,
            Infrastructure.HeaterCycleLogger heaterCycle
        )
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== SMART TERMOREGULATOR ===");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Podešavanja režima i temperature");
                Console.WriteLine("2. Prikaz uređaja i temperatura");
                Console.WriteLine("3. Status grejača");
                Console.WriteLine("4. Pregled logova");
                // Dodaj i opciju za pregled logova ako želiš
                Console.WriteLine("0. Izlaz");
                Console.ResetColor();

                Console.Write("\nIzbor: ");
                var izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        // Dodaj i deviceService kao parametar za validaciju automatske regulacije!
                        PodesavanjaScreen.Prikazi(regulatorService, deviceService);
                        break;
                    case "2":
                        PrikazUredjajaScreen.Prikazi(deviceService, heaterService);
                        break;
                    case "3":
                        GrejacStatusScreen.Prikazi(heaterService);
                        break;
                    case "4":
                        LogScreen.Prikazi(fileLogger, heaterCycle);
                        break;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Doviđenja!");
                        Console.ResetColor();
                        Environment.Exit(0);
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nepoznat izbor! Pokušajte ponovo...");
                        Console.ResetColor();
                        Thread.Sleep(1200);
                        break;
                }
            }
        }
    }
}
