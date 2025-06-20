using Domain;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly List<Device> _devices;

        public DeviceService(List<Device> devices)
        {
            _devices = devices;
        }

        public IEnumerable<Device> GetAllDevices() => _devices;

        public Device? GetDeviceById(string deviceId) =>
            _devices.FirstOrDefault(d => d.Id == deviceId);

        public void UpdateTemperature(string deviceId, double newTemperature)
        {
            var device = _devices.FirstOrDefault(d => d.Id == deviceId);
            if (device != null)
                device.SetTemperature(newTemperature);
        }

        public void StartAllDevices(CancellationToken cancellationToken = default)
        {
            foreach (var device in _devices)
                device.Start(cancellationToken);
        }

        public void StopAllDevices()
        {
            foreach (var device in _devices)
                device.Stop();
        }

        public void StartHeatingAll()
        {
            foreach (var device in _devices)
                device.StartHeating();
        }

        public void StopHeatingAll()
        {
            foreach (var device in _devices)
                device.StopHeating();
        }

        public void SubscribeToTemperatureEvents(Action<string, double> temperatureHandler)
        {
            foreach (var device in _devices)
                device.OnTemperatureChecked += temperatureHandler;
        }

        public void UnsubscribeFromTemperatureEvents(Action<string, double> temperatureHandler)
        {
            foreach (var device in _devices)
                device.OnTemperatureChecked -= temperatureHandler;
        }
    }
}
