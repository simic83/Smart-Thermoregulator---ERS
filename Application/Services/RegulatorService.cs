using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using Domain;
using Infrastructure;

namespace Application.Services
{
    public class RegulatorService : IRegulatorService
    {
        private (int, int) _dayRange;
        private (int, int) _nightRange;
        private double _desiredDayTemperature;
        private double _desiredNightTemperature;

        private readonly IDeviceService _deviceService;
        private readonly IHeaterService _heaterService;
        private readonly FileLogger _logger;

        public string CurrentMode { get; private set; } = "noc";

        public RegulatorService(IDeviceService deviceService, IHeaterService heaterService, FileLogger logger)
        {
            _deviceService = deviceService;
            _heaterService = heaterService;
            _logger = logger;
        }

        public void SetDayNightRanges((int, int) day, (int, int) night)
        {
            _dayRange = day;
            _nightRange = night;
            _logger.Log("INFO", $"Podešen dnevni režim: {day.Item1}-{day.Item2}h, noćni: {night.Item1}-{night.Item2}h");
        }

        public void SetDesiredTemperatures(double day, double night)
        {
            _desiredDayTemperature = day;
            _desiredNightTemperature = night;
            _logger.Log("INFO", $"Podešena temperatura: dan {day}, noć {night}");
        }

        /// <summary>
        /// Poziva se svaki put kada uređaj pošalje novu temperaturu.
        /// </summary>
        public void ProcessTemperature(string deviceId, double value)
        {
            _deviceService.UpdateTemperature(deviceId, value);
            _logger.Log("INFO", $"Uređaj {deviceId} poslao temperaturu: {value:F2}°C");

            var devices = _deviceService.GetAllDevices();
            double avgTemp = devices.Average(d => d.CurrentTemperature);
            double desiredTemp = GetDesiredTemperatureByCurrentTime();

            if (avgTemp < desiredTemp)
            {
                if (!_heaterService.IsHeaterOn())
                {
                    _logger.Log("INFO", "Regulator: Prosečna temperatura ispod željene, šaljem komandu za UKLJUČENJE grejača.");
                    _heaterService.TurnOn();
                    _deviceService.StartHeatingAll();
                }
            }
            else
            {
                if (_heaterService.IsHeaterOn())
                {
                    _logger.Log("INFO", "Regulator: Prosečna temperatura iznad željene, šaljem komandu za ISKLJUČENJE grejača.");
                    _heaterService.TurnOff();
                    _deviceService.StopHeatingAll();
                }
            }
        }

        public void SubscribeToDeviceEvents()
        {
            _deviceService.SubscribeToTemperatureEvents(ProcessTemperature);
        }

        public void UnsubscribeFromDeviceEvents()
        {
            _deviceService.UnsubscribeFromTemperatureEvents(ProcessTemperature);
        }

        // Privatna metoda za određivanje trenutnog režima i željene temperature
        private double GetDesiredTemperatureByCurrentTime()
        {
            int hour = DateTime.Now.Hour;
            if (IsInRange(hour, _dayRange))
            {
                CurrentMode = "dan";
                return _desiredDayTemperature;
            }
            else
            {
                CurrentMode = "noc";
                return _desiredNightTemperature;
            }
        }

        // Helper metoda za proveru da li je sat u opsegu
        private bool IsInRange(int hour, (int, int) range)
        {
            if (range.Item1 < range.Item2)
                return hour >= range.Item1 && hour < range.Item2;
            else
                // Preko ponoći (npr. 22-6)
                return hour >= range.Item1 || hour < range.Item2;
        }
    }
}
