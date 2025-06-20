using System;
using System.Collections.Generic;
using System.Threading;
using Domain;

namespace Application.Interfaces
{
    public interface IDeviceService
    {
        IEnumerable<Device> GetAllDevices();
        void UpdateTemperature(string deviceId, double newTemperature);
        Device? GetDeviceById(string deviceId);

        void StartAllDevices(CancellationToken cancellationToken = default);
        void StopAllDevices();

        void StartHeatingAll();
        void StopHeatingAll();

        void SubscribeToTemperatureEvents(Action<string, double> temperatureHandler);
        void UnsubscribeFromTemperatureEvents(Action<string, double> temperatureHandler);
    }
}
