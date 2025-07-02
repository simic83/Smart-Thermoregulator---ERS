using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.UnosPodataka
{
    public static class UnosOpsegaSati
    {
        public static (int pocetak, int kraj) UnesiOpseg()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║     KONFIGURACIJA DNEVNOG REŽIMA      ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.ResetColor();

            int pocetak = 0;
            int kraj = 0;
            bool validanUnos = false;

            while (!validanUnos)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n▶ Unesite početak dnevnog režima (0-23h): ");
                    Console.ResetColor();
                    pocetak = int.Parse(Console.ReadLine());

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("▶ Unesite kraj dnevnog režima (0-23h): ");
                    Console.ResetColor();
                    kraj = int.Parse(Console.ReadLine());

                    if (pocetak >= 0 && pocetak <= 23 && kraj >= 0 && kraj <= 23 && pocetak != kraj)
                    {
                        validanUnos = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✓ Dnevni režim: {pocetak:00}:00 - {kraj:00}:00");
                        Console.WriteLine($"✓ Noćni režim: {kraj:00}:00 - {pocetak:00}:00");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("✗ Nevalidan unos! Sati moraju biti između 0 i 23 i različiti.");
                        Console.ResetColor();
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ Greška! Molimo unesite broj.");
                    Console.ResetColor();
                }
            }

            return (pocetak, kraj);
        }
    }
}