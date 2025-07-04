using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;

namespace Presentation.Initialization
{
    public static class DeviceInitializer
    {
        public static void CreateTestDevices(IDevicesService devicesService, ILoggerService loggerService)
        {
            string[] sobe = { "Dnevna soba", "Spavaća soba", "Kuhinja", "Kupatilo" };
            var random = new Random();

            for (int i = 0; i < 4; i++)
            {
                var device = new Device($"DEV{i + 1:000}")
                {
                    Temperatura = 18 + random.NextDouble() * 4
                };
                devicesService.RegisterDevice(device);
            }

            loggerService.LogEvent("Sistem pokrenut sa 4 test uređaja");
        }
    }
}
