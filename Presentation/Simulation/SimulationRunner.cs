using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;

namespace Presentation.Simulation
{

    public class SimulationRunner
    {
        private readonly IDevicesService _devicesService;
        private readonly IHeaterService _heaterService;
        private readonly IRegulatorService _regulatorService;

        public SimulationRunner(IDevicesService devicesService, IHeaterService heaterService, IRegulatorService regulatorService)
        {
            _devicesService = devicesService;
            _heaterService = heaterService;
            _regulatorService = regulatorService;
        }

        public async Task RunSimulation(CancellationToken cancellationToken)
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
                        // Ako peć nije uključena, temperatura pada
                        if (!device.JePecUkljucena)
                        {
                            device.Temperatura -= 0.3;
                        }

                        // Dodaj malu random varijaciju
                        var noise = (random.NextDouble() - 0.5) * 0.1;
                        device.Temperatura += noise;

                        _devicesService.UpdateDeviceTemperature(device.Id, device.Temperatura);
                        _regulatorService.ProcessTemperatureReading(device.Id, device.Temperatura);
                    }

                    lastReadingTime = currentTime;
                }

                // Zagrevanje ako je peć uključena - svakih 1 sekundu
                if (_heaterService.IsOn() && (currentTime - lastHeatingUpdateTime).TotalSeconds >= 1)
                {
                    var devices = _devicesService.GetAllDevices();
                    foreach (var device in devices)
                    {
                        device.Temperatura += 0.5;
                        _devicesService.UpdateDeviceTemperature(device.Id, device.Temperatura);
                    }
                    lastHeatingUpdateTime = currentTime;
                }

                await Task.Delay(500, cancellationToken);
            }
        }
    }
}