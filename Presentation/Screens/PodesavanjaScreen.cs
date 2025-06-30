using System;

namespace Presentation.Screens
{
    public static class PodesavanjaScreen
    {
        public static void Prikazi(Application.Services.RegulatorService regulatorService, Application.Services.DeviceService deviceService)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(">> Podešavanja režima i željenih temperatura\n");
            Console.ResetColor();

            int dayStart;
            do
            {
                Console.Write("Unesite početak dnevnog režima (0-23): ");
            } while (!int.TryParse(Console.ReadLine(), out dayStart) || dayStart < 0 || dayStart > 23);

            int dayEnd;
            do
            {
                Console.Write("Unesite kraj dnevnog režima (0-23): ");
            } while (!int.TryParse(Console.ReadLine(), out dayEnd) || dayEnd < 0 || dayEnd > 23);

            var dayRange = (dayStart, dayEnd);
            var nightRange = (dayEnd, dayStart);

            double dayTemp;
            do
            {
                Console.Write("Unesite željenu temperaturu za DAN (5-35°C): ");
            } while (!double.TryParse(Console.ReadLine(), out dayTemp) || dayTemp < 5 || dayTemp > 35);

            double nightTemp;
            do
            {
                Console.Write("Unesite željenu temperaturu za NOĆ (5-35°C): ");
            } while (!double.TryParse(Console.ReadLine(), out nightTemp) || nightTemp < 5 || nightTemp > 35);

            regulatorService.SetDayNightRanges(dayRange, nightRange);
            regulatorService.SetDesiredTemperatures(dayTemp, nightTemp);

            // ODMAH procesiraj temperature svih uređaja
            var devices = deviceService.GetAllDevices();
            foreach (var d in devices)
                regulatorService.ProcessTemperature(d.Id, d.CurrentTemperature);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nUspešno podešen dnevni režim {dayStart}-{dayEnd}h ({dayTemp}°C), noćni {dayEnd}-{dayStart}h ({nightTemp}°C)");
            Console.ResetColor();

            Console.WriteLine("\nPritisnite ENTER za povratak u meni...");
            Console.ReadLine();
        }
    }
}
