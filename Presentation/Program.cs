using System;
using System.Collections.Generic;
using System.Threading;
using Domain;
using Application.Services;
using Infrastructure; // da bi mogao da koristiš FileLogger

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

            //// ===== EP-4 testovi (zakomentarisano) =====
            //var devices = new List<Device>
            //{
            //    new Device("1", 18.5),
            //    new Device("2", 18.8),
            //    new Device("3", 19.0),
            //    new Device("4", 18.7)
            //};

            //var heater = new Heater();

            //var deviceService = new DeviceService(devices);
            //var heaterService = new HeaterService(heater);
            //var regulatorService = new RegulatorService(deviceService, heaterService);

            //regulatorService.SetDayNightRanges((6, 22), (22, 6)); // Dan: 6-22h, Noć: 22-6h
            //regulatorService.SetDesiredTemperatures(21.0, 18.0);  // Dan: 21°C, Noć: 18°C

            //Console.WriteLine("\n=== EP-4: Test automatske regulacije ===");
            //Console.WriteLine($"Trenutno vreme: {DateTime.Now}");
            //Console.WriteLine("Trenutne temperature uređaja:");
            //foreach (var d in devices)
            //    Console.WriteLine($"ID: {d.Id}, Temp: {d.CurrentTemperature}°C");

            //foreach (var d in devices)
            //    regulatorService.ProcessTemperature(d.Id, d.CurrentTemperature);

            //Console.WriteLine("Prosečna temperatura: " + (devices[0].CurrentTemperature + devices[1].CurrentTemperature + devices[2].CurrentTemperature + devices[3].CurrentTemperature) / 4.0 + "°C");
            //Console.WriteLine($"Grejač status posle regulacije: {(heaterService.IsHeaterOn() ? "UKLJUČEN" : "ISKLJUČEN")}");

            //devices[0].SetTemperature(21.5);
            //devices[1].SetTemperature(21.2);
            //devices[2].SetTemperature(21.0);
            //devices[3].SetTemperature(21.1);

            //Console.WriteLine("\nSimulacija: Svi uređaji su sada iznad željene temperature.");
            //foreach (var d in devices)
            //    regulatorService.ProcessTemperature(d.Id, d.CurrentTemperature);

            //Console.WriteLine("Prosečna temperatura: " + (devices[0].CurrentTemperature + devices[1].CurrentTemperature + devices[2].CurrentTemperature + devices[3].CurrentTemperature) / 4.0 + "°C");
            //Console.WriteLine($"Grejač status posle regulacije: {(heaterService.IsHeaterOn() ? "UKLJUČEN" : "ISKLJUČEN")}");

            //Console.WriteLine("\nTest EP-4 uspešno završen.");

            //// ===== EP-5 testovi =====

            //// Inicijalizuj loggere
            //var logger = new FileLogger("dnevnik.txt");
            //var cycleLogger = new HeaterCycleLogger("grejac-log.txt");

            //logger.Log("INFO", "Aplikacija pokrenuta (EP-5 test).");

            //var heaterEp5 = new Heater();
            //logger.Log("INFO", "Grejač kreiran.");

            //// Simuliramo uključivanje grejača
            //heaterEp5.TurnOn();
            //logger.Log("INFO", "Grejač uključen.");

            //Console.WriteLine("Grejač uključen, čeka se 5 sekundi za simulaciju rada...");
            //Thread.Sleep(5000);

            //// Simuliramo isključivanje grejača
            //heaterEp5.TurnOff();
            //logger.Log("INFO", "Grejač isključen.");

            //// Simulacija ciklusa od 5 sekundi
            //DateTime start = DateTime.Now.AddSeconds(-5);
            //DateTime end = DateTime.Now;
            //TimeSpan trajanje = end - start;
            //double resurs = trajanje.TotalSeconds * 0.1; // npr. 0.1 jedinica po sekundi

            //var cycleInfo = new HeaterCycleInfo(
            //    start,      // startTime
            //    end,        // endTime
            //    trajanje,   // duration
            //    resurs      // resourceUsed
            //);
            //cycleLogger.LogCycle(cycleInfo);

            //logger.Log("INFO", "Ciklus rada grejača upisan u grejac-log.txt.");
            //logger.Log("INFO", "Aplikacija završena (EP-5 test).");

            //Console.WriteLine("\nTest EP-5 uspešno završen. Proveri dnevnik.txt i grejac-log.txt za evidenciju događaja.");

            var devices = new List<Device>
            {
                new Device("1", 20.0),
                new Device("2", 20.0),
                new Device("3", 20.0),
                new Device("4", 20.0)
            };

            var heater = new Heater();

            var deviceService = new DeviceService(devices);
            var heaterService = new HeaterService(heater);
            var regulatorService = new RegulatorService(deviceService, heaterService);


            // ===== EP-6 testovi =====
            Console.WriteLine("\nEP-6: Unos režima rada i željenih temperatura.");

            Console.Write("Unesite početak dnevnog režima (0-23): ");
            int dayStart = int.Parse(Console.ReadLine() ?? "6");

            Console.Write("Unesite kraj dnevnog režima (0-23): ");
            int dayEnd = int.Parse(Console.ReadLine() ?? "22");

            // Noćni režim je automatski ostatak dana
            var dayRange = (dayStart, dayEnd);
            var nightRange = (dayEnd, dayStart);

            Console.Write("Unesite željenu temperaturu za DAN: ");
            double dayTemp = double.Parse(Console.ReadLine() ?? "21");

            Console.Write("Unesite željenu temperaturu za NOĆ: ");
            double nightTemp = double.Parse(Console.ReadLine() ?? "18");

            // Pretpostavljamo da su servisi već inicijalizovani (regulatorService...)
            regulatorService.SetDayNightRanges(dayRange, nightRange);
            regulatorService.SetDesiredTemperatures(dayTemp, nightTemp);

            Console.WriteLine($"Režimi su podešeni: DAN {dayStart}-{dayEnd}h na {dayTemp}°C, NOĆ {dayEnd}-{dayStart}h na {nightTemp}°C.");
            Console.WriteLine("EP-6 test završen.");
        }
    }
}
