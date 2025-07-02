using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoggerServices
{
    using Domain.Services;

    public class FileLoggerService : ILoggerService
    {
        private readonly string _logPath = "thermoregulator_log.txt";

        public void LogEvent(string message)
        {
            WriteToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] EVENT: {message}");
        }

        public void LogTemperatureReading(string deviceId, double temperature)
        {
            WriteToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] TEMP: Device {deviceId} - {temperature:F1}°C");
        }

        public void LogHeaterStateChange(bool isOn)
        {
            WriteToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] HEATER: {(isOn ? "UKLJUČENA" : "ISKLJUČENA")}");
        }

        public void LogHeaterCycle(bool isOn, DateTime time)
        {
            WriteToFile($"[{time:yyyy-MM-dd HH:mm:ss}] HEAT-CYCLE: Peć {(isOn ? "UKLJUČENA" : "ISKLJUČENA")} u {time:HH:mm:ss}");
        }

        public void LogError(string error)
        {
            WriteToFile($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {error}");
        }

        private void WriteToFile(string content)
        {
            try
            {
                File.AppendAllText(_logPath, content + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri pisanju u log: {ex.Message}");
            }
        }
    }
}