using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.UnosTemepratura
{
    public static class UnosTemperature
    {
        public static (double dnevna, double nocna) UnesiTemperature()
        {
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

            return (dnevna, nocna);
        }
    }
}