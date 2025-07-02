using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ILoggerService
    {
        void LogEvent(string message);
        void LogTemperatureReading(string deviceId, double temperature);
        void LogHeaterStateChange(bool isOn);
        void LogHeaterCycle(bool isOn, DateTime time);
        void LogError(string error);
    }
}