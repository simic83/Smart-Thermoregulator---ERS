using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.DevicesRepositories
{
    using global::Domain.Models;

    public interface IDeviceRepository
    {
        void Add(Device device);
        Device GetById(string id);
        List<Device> GetAll();
        void Update(Device device);
    }
}