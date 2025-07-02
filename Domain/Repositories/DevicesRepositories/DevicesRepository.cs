using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.DevicesRepositories
{
    using global::Domain.Models;

    public class DevicesRepository : IDeviceRepository
    {
        private readonly Dictionary<string, Device> _devices = new Dictionary<string, Device>();

        public void Add(Device device)
        {
            if (!_devices.ContainsKey(device.Id))
            {
                _devices.Add(device.Id, device);
            }
        }

        public Device GetById(string id)
        {
            return _devices.ContainsKey(id) ? _devices[id] : null;
        }

        public List<Device> GetAll()
        {
            return _devices.Values.ToList();
        }

        public void Update(Device device)
        {
            if (_devices.ContainsKey(device.Id))
            {
                _devices[device.Id] = device;
            }
        }
    }
}