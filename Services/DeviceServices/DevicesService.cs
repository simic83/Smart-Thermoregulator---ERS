using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Domain.Repositories.DevicesRepositories;

namespace Services.DeviceServices
{
    using Domain.Models;
    using Domain.Services;
    using Domain.Repositories.DevicesRepositories;

    public class DevicesService : IDevicesService
    {
        private readonly IDeviceRepository _repository;
        private readonly ILoggerService _logger;

        public DevicesService(IDeviceRepository repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void RegisterDevice(Device device)
        {
            _repository.Add(device);
            _logger.LogEvent($"Registrovan uredjaj: {device.Id}");
        }

        public List<Device> GetAllDevices()
        {
            return _repository.GetAll();
        }

        public void UpdateDeviceTemperature(string deviceId, double temperature)
        {
            var device = _repository.GetById(deviceId);
            if (device != null)
            {
                device.Temperatura = temperature;
                device.PoslednjeOcitavanje = DateTime.Now;
                _repository.Update(device);
                _logger.LogTemperatureReading(deviceId, temperature);
            }
        }

        public void NotifyHeaterStateChange(bool isHeaterOn)
        {
            var devices = _repository.GetAll();
            foreach (var device in devices)
            {
                device.JePecUkljucena = isHeaterOn;
                _repository.Update(device);
            }
            _logger.LogEvent($"Svi uredjaji obavešteni o statusu peći: {(isHeaterOn ? "UKLJUČENA" : "ISKLJUČENA")}");
        }
    }
}
