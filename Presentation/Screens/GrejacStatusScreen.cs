using System;

namespace Presentation.Screens
{
    public static class GrejacStatusScreen
    {
        public static void Prikazi(Application.Services.HeaterService heaterService)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(">> Status Grejača\n");
            Console.ResetColor();

            bool jeUkljucen = heaterService.IsHeaterOn();
            double potrosnja = heaterService.GetTotalResourceUsedLive();
            double radnoVreme = heaterService.GetTotalWorkingTimeLive().TotalMinutes;

            Console.Write("Grejač je trenutno: ");
            Console.ForegroundColor = jeUkljucen ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(jeUkljucen ? "UKLJUČEN" : "ISKLJUČEN");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Ukupna potrošnja: {potrosnja:F2} kWh");
            Console.WriteLine($"Ukupno radno vreme: {radnoVreme:F1} minuta");
            Console.ResetColor();

            Console.WriteLine("\nPritisnite ENTER za povratak u meni...");
            Console.ReadLine();
        }
    }
}
