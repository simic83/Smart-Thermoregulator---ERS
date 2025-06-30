using System;

namespace Presentation.Screens
{
    public static class PrikazUredjajaScreen
    {
        public static void Prikazi(Application.Services.DeviceService deviceService, Application.Services.HeaterService heaterService)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(">> Prikaz svih uređaja\n");
            Console.ResetColor();

            var devices = deviceService.GetAllDevices();
            bool grejacUkljucen = heaterService.IsHeaterOn();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("ID\tTemperatura\tGrejanje");
            Console.ResetColor();

            foreach (var d in devices)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(d.Id + "\t");
                Console.ResetColor();

                Console.ForegroundColor = d.CurrentTemperature >= 21.0 ? ConsoleColor.Cyan : ConsoleColor.Yellow;
                Console.Write($"{d.CurrentTemperature:F2}°C\t\t");
                Console.ResetColor();

                Console.ForegroundColor = grejacUkljucen ? ConsoleColor.Red : ConsoleColor.Gray;
                Console.WriteLine(grejacUkljucen ? "DA" : "NE");
                Console.ResetColor();
            }

            Console.WriteLine("\nPritisnite ENTER za povratak u meni...");
            Console.ReadLine();
        }
    }
}
