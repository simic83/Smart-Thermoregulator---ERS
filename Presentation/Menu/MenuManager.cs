using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Presentation.Options;
using System.Data;

namespace Presentation.Menu
{

    public class MenuManager
    {
        private readonly IDevicesService _devicesService;
        private readonly IHeaterService _heaterService;
        private readonly IRegulatorService _regulatorService;
        private readonly ILoggerService _loggerService;

        public MenuManager(
            IDevicesService devicesService,
            IHeaterService heaterService,
            IRegulatorService regulatorService,
            ILoggerService loggerService)
        {
            _devicesService = devicesService;
            _heaterService = heaterService;
            _regulatorService = regulatorService;
            _loggerService = loggerService;
        }

        public async Task ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowHeader();
                ShowMenuOptions();

                var choice = Console.ReadLine();
                if (!await ProcessMenuChoice(choice))
                    break;
            }
        }

        private void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║              SMART THERMOREGULATOR SYSTEM                     ║
║                    Version 1.0                                ║
╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private void ShowMenuOptions()
        {
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
        }

        private async Task<bool> ProcessMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    var workModeOption = new WorkModeOption(_regulatorService);
                    workModeOption.Execute();
                    return true;

                case "2":
                    var tempOption = new TemperatureOption(_regulatorService);
                    tempOption.Execute();
                    return true;

                case "3":
                    var simOption = new SimulationOption(_devicesService, _heaterService, _regulatorService);
                    await simOption.Execute();
                    return true;

                case "4":
                    var statusOption = new StatusOption(_devicesService, _heaterService, _regulatorService);
                    statusOption.Execute();
                    return true;

                case "5":
                    var logsOption = new LogsOption();
                    logsOption.Execute();
                    return true;

                case "6":
                    var statsOption = new StatisticsOption(_heaterService);
                    statsOption.Execute();
                    return true;

                case "7":
                    var exitOption = new ExitOption();
                    return !exitOption.Execute();

                default:
                    ShowInvalidOption();
                    return true;
            }
        }

        private void ShowInvalidOption()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n✗ Nevalidna opcija!");
            Console.ResetColor();
            Thread.Sleep(1500);
        }
    }
}