using Domain.Models;
using Domain.Repositories.DevicesRepositories;
using Domain.Services;
using Services.DeviceServices;
using Services.HeaterServices;
using Services.LoggerServices;
using Services.RegulatorServices;
using Presentation.Menu;
using Presentation.Initialization;

namespace Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var deviceRepository = new DevicesRepository();
            var loggerService = new FileLoggerService();
            var devicesService = new DevicesService(deviceRepository, loggerService);
            var heaterService = new HeaterService(loggerService);
            var regulatorService = new RegulatorService(heaterService, devicesService, loggerService);

            DeviceInitializer.CreateTestDevices(devicesService, loggerService);

            // Start Menu
            var menuManager = new MenuManager(devicesService, heaterService, regulatorService, loggerService);
            await menuManager.ShowMainMenu();
        }
    }
}