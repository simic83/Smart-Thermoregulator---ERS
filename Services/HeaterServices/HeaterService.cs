using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;

namespace Services.HeaterServices
{
    public class HeaterService : IHeaterService
    {
        private readonly Heater _heater;
        private readonly ILoggerService _logger;

        public HeaterService(ILoggerService logger)
        {
            _heater = new Heater();
            _logger = logger;
        }

        public void TurnOn()
        {
            if (!_heater.JeUkljucen)
            {
                _heater.JeUkljucen = true;
                _heater.VremePocetkaRada = DateTime.Now;
                _logger.LogHeaterStateChange(true);
                _logger.LogHeaterCycle(true, DateTime.Now);
            }
        }

        public void TurnOff()
        {
            if (_heater.JeUkljucen && _heater.VremePocetkaRada.HasValue)
            {
                var endTime = DateTime.Now;
                var period = new RadniPeriod
                {
                    VremePocetka = _heater.VremePocetkaRada.Value,
                    VremeKraja = endTime,
                    TrajanjeSati = (endTime - _heater.VremePocetkaRada.Value).TotalSeconds / 3600.0,
                    PotroseniResursi = CalculateResourceConsumption(_heater.VremePocetkaRada.Value, endTime)
                };

                _heater.RadniPeriodi.Add(period);
                _heater.JeUkljucen = false;
                _heater.VremePocetkaRada = null;

                _logger.LogHeaterStateChange(false);
                _logger.LogHeaterCycle(false, endTime);
                _logger.LogEvent($"Peć radila: {period.TrajanjeSati * 3600:F0} sekundi, potrošeno: {period.PotroseniResursi:F3} kWh");
            }
        }

        public bool IsOn()
        {
            return _heater.JeUkljucen;
        }

        public double GetTotalWorkingHours()
        {
            // Vraća ukupno vreme u sekundama
            return _heater.RadniPeriodi.Sum(p => p.TrajanjeSati * 3600); // Konvertuje sate u sekunde
        }

        public double GetResourceConsumption()
        {
            return _heater.RadniPeriodi.Sum(p => p.PotroseniResursi);
        }

        private double CalculateResourceConsumption(DateTime start, DateTime end)
        {
            // Za demo: računa po sekundama umesto po satima
            var seconds = (end - start).TotalSeconds;
            return seconds * 0.01; // 0.01 kWh po sekundi rada (za brži prikaz potrošnje)
        }
    }
}