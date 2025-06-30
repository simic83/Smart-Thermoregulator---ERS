using System;
using Domain;
using Application.Interfaces;
using Infrastructure;

namespace Application.Services
{
    public class HeaterService : IHeaterService
    {
        private readonly Heater _heater;
        private readonly HeaterCycleLogger _cycleLogger;
        private readonly FileLogger _logger;

        public HeaterService(Heater heater, HeaterCycleLogger cycleLogger, FileLogger logger)
        {
            _heater = heater ?? throw new ArgumentNullException(nameof(heater));
            _cycleLogger = cycleLogger ?? throw new ArgumentNullException(nameof(cycleLogger));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void TurnOn()
        {
            _heater.TurnOn();
            _logger.Log("INFO", "Grejač UKLJUČEN.");
        }

        public void TurnOff()
        {
            DateTime? lastOnTime = _heater.LastOnTime;
            _heater.TurnOff();
            _logger.Log("INFO", "Grejač ISKLJUČEN.");

            if (lastOnTime.HasValue)
            {
                var now = DateTime.Now;
                var duration = now - lastOnTime.Value;
                var used = duration.TotalMinutes * 0.1; // Tvoja formula

                var cycle = new HeaterCycleInfo(
                    startTime: lastOnTime.Value,
                    endTime: now,
                    duration: duration,
                    resourceUsed: used
                );
                _cycleLogger.LogCycle(cycle);
            }
        }

        public bool IsHeaterOn() => _heater.IsOn;
        public DateTime? GetLastOnTime() => _heater.LastOnTime;
        public TimeSpan GetTotalWorkingTime() => _heater.TotalWorkingTime;
        public double GetTotalResourceUsed() => _heater.TotalResourceUsed;
        public TimeSpan GetTotalWorkingTimeLive() => _heater.GetTotalWorkingTimeLive();
        public double GetTotalResourceUsedLive() => _heater.GetTotalResourceUsedLive();
        public void Reset() => _heater.Reset();
    }
}
