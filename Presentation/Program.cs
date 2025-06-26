using System;
using System.Collections.Generic;
using System.Threading;
using Domain;
using Application.Services;

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

            //// ===== EP-3 testovi (zakomentarisano) =====
            //var heater = new Heater();

            //Console.WriteLine("Grejač je trenutno: " + (heater.IsOn ? "UKLJUČEN" : "ISKLJUČEN"));
            //heater.TurnOn();
            //Console.WriteLine("Čeka se 5 sekundi da simuliramo rad...");
            //Thread.Sleep(5000); // Simulacija rada grejača (5 sekundi)

            //heater.TurnOff();
            //Console.WriteLine("Grejač je trenutno: " + (heater.IsOn ? "UKLJUČEN" : "ISKLJUČEN"));

            //Console.WriteLine($"\nVreme poslednjeg uključivanja: {heater.LastOnTime}");
            //Console.WriteLine($"Ukupno radno vreme grejača: {heater.TotalWorkingTime.TotalSeconds} sekundi");
            //Console.WriteLine($"Ukupno potrošeni resursi: {heater.TotalResourceUsed:F2} kW");

            //Console.WriteLine("\nTest EP-3 uspešno završen.");


            // ===== EP-4 testovi =====

            // Inicijalizacija uređaja
            var devices = new List<Device>
            {
                new Device("1", 18.5),
                new Device("2", 18.8),
                new Device("3", 19.0),
                new Device("4", 18.7)
            };

            var heater = new Heater();

            // Kreiramo servise
            var deviceService = new DeviceService(devices);
            var heaterService = new HeaterService(heater);
            var regulatorService = new RegulatorService(deviceService, heaterService);

            // Postavljamo željenu temperaturu i dnevni/noćni režim
            regulatorService.SetDayNightRanges((6, 22), (22, 6)); // Dan: 6-22h, Noć: 22-6h
            regulatorService.SetDesiredTemperatures(21.0, 18.0);  // Dan: 21°C, Noć: 18°C

            // Simulacija - prosečna temperatura je 18.75°C, dan je, željena je 21°C → grejač treba da se upali
            Console.WriteLine("\n=== EP-4: Test automatske regulacije ===");
            Console.WriteLine($"Trenutno vreme: {DateTime.Now}");
            Console.WriteLine("Trenutne temperature uređaja:");
            foreach (var d in devices)
                Console.WriteLine($"ID: {d.Id}, Temp: {d.CurrentTemperature}°C");

            // "Ručna" simulacija - kao da su svi uređaji poslali temperaturu regulatoru
            foreach (var d in devices)
                regulatorService.ProcessTemperature(d.Id, d.CurrentTemperature);

            Console.WriteLine("Prosečna temperatura: " + (devices[0].CurrentTemperature + devices[1].CurrentTemperature + devices[2].CurrentTemperature + devices[3].CurrentTemperature) / 4.0 + "°C");
            Console.WriteLine($"Grejač status posle regulacije: {(heaterService.IsHeaterOn() ? "UKLJUČEN" : "ISKLJUČEN")}");

            // Sada povećaj temperaturu uređaja iznad željene i simuliraj ponovno procesiranje
            devices[0].SetTemperature(21.5);
            devices[1].SetTemperature(21.2);
            devices[2].SetTemperature(21.0);
            devices[3].SetTemperature(21.1);

            Console.WriteLine("\nSimulacija: Svi uređaji su sada iznad željene temperature.");
            foreach (var d in devices)
                regulatorService.ProcessTemperature(d.Id, d.CurrentTemperature);

            Console.WriteLine("Prosečna temperatura: " + (devices[0].CurrentTemperature + devices[1].CurrentTemperature + devices[2].CurrentTemperature + devices[3].CurrentTemperature) / 4.0 + "°C");
            Console.WriteLine($"Grejač status posle regulacije: {(heaterService.IsHeaterOn() ? "UKLJUČEN" : "ISKLJUČEN")}");

            Console.WriteLine("\nTest EP-4 uspešno završen.");
        }
    }
}
