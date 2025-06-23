using System;
using Domain;
using Application.Interfaces;

namespace Application.Services
{
    public class HeaterService : IHeaterService
    {
        private readonly Heater _heater;

        public HeaterService(Heater heater)
        {
            _heater = heater ?? throw new ArgumentNullException(nameof(heater));
        }

        public void TurnOn()
        {
            _heater.TurnOn();
        }

        public void TurnOff()
        {
            _heater.TurnOff();
        }

        public bool IsHeaterOn()
        {
            return _heater.IsOn;
        }

        public DateTime? GetLastOnTime()
        {
            return _heater.LastOnTime;
        }

        public TimeSpan GetTotalWorkingTime()
        {
            return _heater.TotalWorkingTime;
        }

        public double GetTotalResourceUsed()
        {
            return _heater.TotalResourceUsed;
        }

        // NOVE METODE za trenutno (uživo) stanje:
        public TimeSpan GetTotalWorkingTimeLive()
        {
            return _heater.GetTotalWorkingTimeLive();
        }

        public double GetTotalResourceUsedLive()
        {
            return _heater.GetTotalResourceUsedLive();
        }

        public void Reset()
        {
            _heater.Reset();
        }
    }
}
