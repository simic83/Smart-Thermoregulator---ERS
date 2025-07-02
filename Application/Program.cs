using Domain.Models;
using Domain.Repositories.DevicesRepositories;
using Domain.Services;
using Services.DeviceServices;
using Services.HeaterServices;
using Services.LoggerServices;
using Services.RegulatorServices;
using Presentation.UnosPodataka;
using Presentation.UnosTemepratura;

namespace Application
{
    class Program
    {
        private static IDevicesService _devicesService;
        private static IHeaterService _heaterService;
        private static IRegulatorService _regulatorService;
        private static ILoggerService _loggerService;
        private static bool _simulationRunning = false;
        private static CancellationTokenSource _cancellationTokenSource;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Inicijalizacija
            var deviceRepository = new DevicesRepository();
            _loggerService = new FileLoggerService();
            _devicesService = new DevicesService(deviceRepository, _loggerService);
            _heaterService = new HeaterService(_loggerService);
            _regulatorService = new RegulatorService(_heaterService, _devicesService, _loggerService);

            // Kreiranje test uređaja pri pokretanju
            CreateTestDevices();

            // Glavni meni
            await MainMenu();
        }

        static async Task MainMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowHeader();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                      GLAVNI MENI                              ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n1. 🕐 Podešavanje režima rada");
                Console.WriteLine("2. 🌡️  Podešavanje temperatura");
                Console.WriteLine("3. 📊 Pokreni/Zaustavi simulaciju");
                Console.WriteLine("4. 📋 Pregled trenutnog statusa");
                Console.WriteLine("5. 📄 Pregled logova");
                Console.WriteLine("6. 📈 Statistika potrošnje");
                Console.WriteLine("7. 🚪 Izlaz");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n▶ Izaberite opciju: ");
                Console.ResetColor();

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ConfigureWorkMode();
                        break;
                    case "2":
                        ConfigureTemperatures();
                        break;
                    case "3":
                        await ToggleSimulation();
                        break;
                    case "4":
                        ShowCurrentStatus();
                        break;
                    case "5":
                        ShowLogs();
                        break;
                    case "6":
                        ShowStatistics();
                        break;
                    case "7":
                        if (ConfirmExit())
                            return;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n✗ Nevalidna opcija!");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        break;
                }
            }
        }

        static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║              SMART THERMOREGULATOR SYSTEM                     ║
║                    Version 1.0                                ║
╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void CreateTestDevices()
        {
            string[] sobe = { "Dnevna soba", "Spavaća soba", "Kuhinja", "Kupatilo" };
            var random = new Random();

            for (int i = 0; i < 4; i++)
            {
                var device = new Device($"DEV{i + 1:000}")
                {
                    Temperatura = 18 + random.NextDouble() * 4 // 18-22°C
                };
                _devicesService.RegisterDevice(device);
            }

            _loggerService.LogEvent("Sistem pokrenut sa 4 test uređaja");
        }

        static void ConfigureWorkMode()
        {
            Console.Clear();
            var (pocetak, kraj) = UnosOpsegaSati.UnesiOpseg();

            // Čuvamo trenutne temperature ako su već podešene
            var currentDayTemp = _regulatorService.GetTargetTemperature();
            var currentNightTemp = currentDayTemp; // Pojednostavljeno

            _regulatorService.ConfigureWorkMode(pocetak, kraj, currentDayTemp, currentNightTemp);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Režim rada uspešno podešen!");
            Console.ResetColor();

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }

        static void ConfigureTemperatures()
        {
            Console.Clear();
            var (dnevna, nocna) = UnosTemperature.UnesiTemperature();

            // Preuzimamo trenutne sate ako su već podešeni
            var currentMode = _regulatorService.GetCurrentMode();
            _regulatorService.ConfigureWorkMode(
                _regulatorService.GetCurrentMode() == Domain.Enums.RezimRada.Dnevni ? 6 : 22, // Default vrednosti
                _regulatorService.GetCurrentMode() == Domain.Enums.RezimRada.Dnevni ? 22 : 6,
                dnevna,
                nocna
            );

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Temperature uspešno podešene!");
            Console.ResetColor();

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }

        static async Task ToggleSimulation()
        {
            if (!_simulationRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n▶ Pokretanje simulacije...");
                Console.ResetColor();

                _simulationRunning = true;
                _cancellationTokenSource = new CancellationTokenSource();
                _regulatorService.StartRegulation();

                // Pokreni simulaciju u pozadini
                _ = Task.Run(() => RunSimulation(_cancellationTokenSource.Token));

                Thread.Sleep(1500);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n■ Zaustavljanje simulacije...");
                Console.ResetColor();

                _simulationRunning = false;
                _cancellationTokenSource?.Cancel();

                Thread.Sleep(1500);
            }
        }

        static async Task RunSimulation(CancellationToken cancellationToken)
        {
            var lastReadingTime = DateTime.Now;
            var lastHeatingUpdateTime = DateTime.Now;
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;

                // Očitavanje temperature svakih 3 sekunde
                if ((currentTime - lastReadingTime).TotalSeconds >= 3)
                {
                    var devices = _devicesService.GetAllDevices();

                    foreach (var device in devices)
                    {
                        // Ako peć nije uključena, temperatura brže pada za demo
                        if (!device.JePecUkljucena)
                        {
                            device.Temperatura -= 0.3; // Brži pad temperature za demo
                        }

                        // Dodaj malu random varijaciju
                        var noise = (random.NextDouble() - 0.5) * 0.1; // ±0.05°C
                        device.Temperatura += noise;

                        _devicesService.UpdateDeviceTemperature(device.Id, device.Temperatura);
                        _regulatorService.ProcessTemperatureReading(device.Id, device.Temperatura);
                    }

                    lastReadingTime = currentTime;
                }

                // BRŽE zagrevanje za demo - svakih 1 sekund povećava za 0.5°C
                if (_heaterService.IsOn() && (currentTime - lastHeatingUpdateTime).TotalSeconds >= 1)
                {
                    var devices = _devicesService.GetAllDevices();
                    foreach (var device in devices)
                    {
                        device.Temperatura += 0.5; // Brže povećanje za demo (0.5°C/s)
                        _devicesService.UpdateDeviceTemperature(device.Id, device.Temperatura);
                    }
                    lastHeatingUpdateTime = currentTime;
                }

                await Task.Delay(500, cancellationToken);
            }
        }

        static void ShowCurrentStatus()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var refreshTask = Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.Clear();
                    ShowSystemStatus(_regulatorService, _heaterService);

                    var devices = _devicesService.GetAllDevices();
                    foreach (var device in devices)
                    {
                        ShowDeviceStatus(device);
                    }

                    Console.WriteLine("\n\nPritisnite bilo koji taster za povratak...");

                    // Proveri da li je pritisnut taster bez blokiranja
                    for (int i = 0; i < 10; i++) // Proveri 10 puta u sekundi
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

            // Čeka pritisak tastera i prekida task
            Console.ReadKey(true);
            cancellationTokenSource.Cancel();

            try
            {
                refreshTask.Wait(500); // Maksimalno čeka 500ms
            }
            catch { }
        }

        static void ShowLogs()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    PREGLED LOGOVA                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                if (File.Exists("thermoregulator_log.txt"))
                {
                    var lines = File.ReadAllLines("thermoregulator_log.txt");
                    var lastLines = lines.TakeLast(30).ToArray(); // Poslednja 30 linija

                    Console.WriteLine($"\nPoslednja {lastLines.Length} događaja:\n");

                    foreach (var line in lastLines)
                    {
                        if (line.Contains("ERROR"))
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (line.Contains("HEAT-CYCLE"))
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else if (line.Contains("HEATER"))
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (line.Contains("TEMP"))
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        else
                            Console.ForegroundColor = ConsoleColor.Gray;

                        Console.WriteLine(line);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("\nLog fajl još nije kreiran.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nGreška pri čitanju log fajla: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPritisnite bilo koji taster za povratak...");
            Console.ReadKey();
        }

        static void ShowStatistics()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 STATISTIKA POTROŠNJE                          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            var totalSeconds = _heaterService.GetTotalWorkingHours(); // Sada vraća sekunde
            var totalConsumption = _heaterService.GetResourceConsumption();
            var cost = totalConsumption * 15; // 15 RSD po kWh

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

        static bool ConfirmExit()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDa li ste sigurni da želite da izađete? (D/N)");
            Console.ResetColor();

            var key = Console.ReadKey(true);
            return key.Key == ConsoleKey.D;
        }

        static void ShowSystemStatus(IRegulatorService regulatorService, IHeaterService heaterService)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    STATUS SISTEMA                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n⏰ Vreme: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine($"📅 Režim rada: {regulatorService.GetCurrentMode()}");
            Console.WriteLine($"🎯 Ciljna temperatura: {regulatorService.GetTargetTemperature()}°C");

            // Dodaj prikaz prosečne temperature
            var devices = _devicesService.GetAllDevices();
            var avgTemp = devices.Count > 0 ? devices.Average(d => d.Temperatura) : 0;
            Console.WriteLine($"📊 Prosečna temperatura: {avgTemp:F1}°C");

            if (heaterService.IsOn())
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

            Console.WriteLine($"⚡ Ukupno vreme rada: {heaterService.GetTotalWorkingHours():F0} sekundi");
            Console.WriteLine($"💡 Potrošnja: {heaterService.GetResourceConsumption():F3} kWh");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    TEMPERATURA UREĐAJA                        ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void ShowDeviceStatus(Device device)
        {
            Console.Write($"\n📡 {device.Id}: ");

            // Boja temperature u zavisnosti od vrednosti
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

            // Grafički prikaz temperature
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