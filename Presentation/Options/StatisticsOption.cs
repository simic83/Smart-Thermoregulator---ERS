using System;
using Domain.Services;

namespace Presentation.Options
{
    public class StatisticsOption
    {
        private readonly IHeaterService _heaterService;

        public StatisticsOption(IHeaterService heaterService)
        {
            _heaterService = heaterService;
        }

        public void Execute()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 STATISTIKA POTROŠNJE                          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            var totalSeconds = _heaterService.GetTotalWorkingHours();
            var totalConsumption = _heaterService.GetResourceConsumption();
            var cost = totalConsumption * 15;

            Console.WriteLine($"\n📊 IZVEŠTAJ O POTROŠNJI:\n");
            Console.WriteLine($"⏱️  Ukupno vreme rada peći: {totalSeconds:F0} sekundi");
            Console.WriteLine($"⚡ Ukupna potrošnja: {totalConsumption:F3} kWh");
            Console.WriteLine($"💰 Procenjeni trošak: {cost:F2} RSD");

            if (totalSeconds > 0)
            {
                Console.WriteLine($"\n📈 Prosečna potrošnja: {totalConsumption / totalSeconds * 60:F3} kWh/min");
            }

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }
    }
}
