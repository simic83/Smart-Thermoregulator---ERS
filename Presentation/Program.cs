using System;
using System.Collections.Generic;
using System.Threading;
using Domain;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kreiramo listu uređaja
            var devices = new List<Device>
            {
                new Device("1", 22.0),
                new Device("2", 20.5),
                new Device("3", 19.0),
                new Device("4", 21.3)
            };

            // Ispisujemo podatke o uređajima
            Console.WriteLine("Pregled registrovanih uređaja:");
            foreach (var device in devices)
            {
                Console.WriteLine($"ID: {device.Id}, Trenutna temperatura: {device.CurrentTemperature}°C");
            }

            // Pauza od 3 minuta (10 sekundi za brzi test)
            Console.WriteLine("\nČekam 3 minuta (ili skrati na 10 sekundi za brži test)...");
            Thread.Sleep(TimeSpan.FromSeconds(10));

            // Simulacija slanja temperature regulatoru
            foreach (var device in devices)
            {
                Console.WriteLine($"Uređaj {device.Id}: uspešno poslata temperatura {device.CurrentTemperature}°C regulatoru.");
            }

            Console.WriteLine("\nTest EP-2 uspešno završen.");
        }
    }
}
