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
            //// ===== EP-2 testovi (zakomentarisano) =====
            //var devices = new List<Device>
            //{
            //    new Device("1", 22.0),
            //    new Device("2", 20.5),
            //    new Device("3", 19.0),
            //    new Device("4", 21.3)
            //};

            //Console.WriteLine("Pregled registrovanih uređaja:");
            //foreach (var device in devices)
            //{
            //    Console.WriteLine($"ID: {device.Id}, Trenutna temperatura: {device.CurrentTemperature}°C");
            //}

            //Console.WriteLine("\nČekam 3 minuta (ili skrati na 10 sekundi za brži test)...");
            //Thread.Sleep(TimeSpan.FromSeconds(10));

            //foreach (var device in devices)
            //{
            //    Console.WriteLine($"Uređaj {device.Id}: uspešno poslata temperatura {device.CurrentTemperature}°C regulatoru.");
            //}

            //Console.WriteLine("\nTest EP-2 uspešno završen.");

            // ===== EP-3 testovi =====

            var heater = new Heater();

            Console.WriteLine("Grejač je trenutno: " + (heater.IsOn ? "UKLJUČEN" : "ISKLJUČEN"));
            heater.TurnOn();
            Console.WriteLine("Čeka se 5 sekundi da simuliramo rad...");
            Thread.Sleep(5000); // Simulacija rada grejača (5 sekundi)

            heater.TurnOff();
            Console.WriteLine("Grejač je trenutno: " + (heater.IsOn ? "UKLJUČEN" : "ISKLJUČEN"));

            Console.WriteLine($"\nVreme poslednjeg uključivanja: {heater.LastOnTime}");
            Console.WriteLine($"Ukupno radno vreme grejača: {heater.TotalWorkingTime.TotalSeconds} sekundi");
            Console.WriteLine($"Ukupno potrošeni resursi: {heater.TotalResourceUsed:F2} kW");

            Console.WriteLine("\nTest EP-3 uspešno završen.");
        }
    }
}
