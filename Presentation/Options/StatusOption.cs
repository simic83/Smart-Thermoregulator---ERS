using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;

namespace Presentation.Options
{
    public class StatusOption
    {
        private readonly IDevicesService _devicesService;
        private readonly IHeaterService _heaterService;
        private readonly IRegulatorService _regulatorService;

        public StatusOption(IDevicesService devicesService, IHeaterService heaterService, IRegulatorService regulatorService)
        {
            _devicesService = devicesService;
            _heaterService = heaterService;
            _regulatorService = regulatorService;
        }

        public void Execute()
        {
            var cancellationTokenSource = new System.Threading.CancellationTokenSource();

            var refreshTask = Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.Clear();
                    ShowSystemStatus();
                    ShowAllDevices();

                    Console.WriteLine("\n\nPritisnite bilo koji taster za povratak...");

                    for (int i = 0; i < 10; i++)
                    {
                        if (Console.KeyAvailable)
                        {
                            cancellationTokenSource.Cancel();
                            break;
                        }
                        await Task.Delay(100);
                    }
                }
            });

            Console.ReadKey(true);
            cancellationTokenSource.Cancel();

            try
            {
                refreshTask.Wait(500);
            }
            catch { }
        }

        private void ShowSystemStatus()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    STATUS SISTEMA                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n⏰ Vreme: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine($"📅 Režim rada: {_regulatorService.GetCurrentMode()}");
            Console.WriteLine($"🎯 Ciljna temperatura: {_regulatorService.GetTargetTemperature()}°C");

            var devices = _devicesService.GetAllDevices();
            var avgTemp = devices.Count > 0 ? devices.Average(d => d.Temperatura) : 0;
            Console.WriteLine($"📊 Prosečna temperatura: {avgTemp:F1}°C");

            if (_heaterService.IsOn())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"🔥 Peć: UKLJUČENA");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"❄️  Peć: ISKLJUČENA");
            }
            Console.ResetColor();

            Console.WriteLine($"⚡ Ukupno vreme rada: {_heaterService.GetTotalWorkingHours():F0} sekundi");
            Console.WriteLine($"💡 Potrošnja: {_heaterService.GetResourceConsumption():F3} kWh");
        }

        private void ShowAllDevices()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    TEMPERATURA UREĐAJA                        ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            var devices = _devicesService.GetAllDevices();
            foreach (var device in devices)
            {
                ShowDeviceStatus(device);
            }
        }

        private void ShowDeviceStatus(Device device)
        {
            Console.Write($"\n📡 {device.Id}: ");

            if (device.Temperatura < 18)
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (device.Temperatura < 22)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (device.Temperatura < 25)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"{device.Temperatura:F2}°C");
            Console.ResetColor();

            Console.Write(" [");
            int barLength = (int)((device.Temperatura - 15) * 2);
            barLength = Math.Max(0, Math.Min(20, barLength));

            for (int i = 0; i < 20; i++)
            {
                if (i < barLength)
                {
                    if (device.Temperatura < 20)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (device.Temperatura < 24)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("█");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("░");
                }
            }
            Console.ResetColor();
            Console.Write("]");

            if (device.JePecUkljucena)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" 🔥");
            }
            Console.ResetColor();
        }
    }
}
