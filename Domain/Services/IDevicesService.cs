using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    using global::Domain.Models;

    public interface IDevicesService
    {
        void RegisterDevice(Device device);
        List<Device> GetAllDevices();
        void UpdateDeviceTemperature(string deviceId, double temperature);
        void NotifyHeaterStateChange(bool isHeaterOn);
    }
}
