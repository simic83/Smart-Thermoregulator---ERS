using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RegulatorServices
{
    using Domain.Models;
    using Domain.Services;
    using Domain.Enums;
    using Domain.PomocneMetode.RezimiRada;
    using Domain.PomocneMetode.Temperature;

    public class RegulatorService : IRegulatorService
    {
        private readonly Regulator _regulator;
        private readonly IHeaterService _heaterService;
        private readonly IDevicesService _devicesService;
        private readonly ILoggerService _logger;
        private bool _isConfigured = false;

        public RegulatorService(IHeaterService heaterService, IDevicesService devicesService, ILoggerService logger)
        {
            _regulator = new Regulator();
            _heaterService = heaterService;
            _devicesService = devicesService;
            _logger = logger;

            // Default vrednosti
            _regulator.PocetakDnevnogRezima = 6;
            _regulator.KrajDnevnogRezima = 22;
            _regulator.TemperaturaDnevna = 22;
            _regulator.TemperaturaNocna = 18;
        }

        public void ConfigureWorkMode(int dayStart, int dayEnd, double dayTemp, double nightTemp)
        {
            _regulator.PocetakDnevnogRezima = dayStart;
            _regulator.KrajDnevnogRezima = dayEnd;
            _regulator.TemperaturaDnevna = dayTemp;
            _regulator.TemperaturaNocna = nightTemp;
            _isConfigured = true;

            _logger.LogEvent($"Konfigurisan režim rada - Dnevni: {dayStart}h-{dayEnd}h ({dayTemp}°C), Noćni: ({nightTemp}°C)");
        }

        public void ProcessTemperatureReading(string deviceId, double temperature)
        {
            var reading = new TemperaturnoPocitavanje
            {
                DeviceId = deviceId,
                Temperatura = temperature,
                Vreme = DateTime.Now
            };

            _regulator.PocitanjaTemperature.Add(reading);

            // Čuva samo poslednja 100 očitavanja
            if (_regulator.PocitanjaTemperature.Count > 100)
            {
                _regulator.PocitanjaTemperature.RemoveAt(0);
            }

            if (_isConfigured)
            {
                RegulateTemperature();
            }
        }

        public RezimRada GetCurrentMode()
        {
            _regulator.TrenutniRezim = OdredjivanjeRezima.OdrediRezim(_regulator.PocetakDnevnogRezima, _regulator.KrajDnevnogRezima);
            return _regulator.TrenutniRezim;
        }

        public double GetTargetTemperature()
        {
            var mode = GetCurrentMode();
            return mode == RezimRada.Dnevni ? _regulator.TemperaturaDnevna : _regulator.TemperaturaNocna;
        }

        public void StartRegulation()
        {
            _logger.LogEvent("Regulacija započeta");
        }

        private void RegulateTemperature()
        {
            var prosecnaTemp = RacunanjeProsecneTemperature.IzracunajProsek(_regulator.PocitanjaTemperature);
            var ciljnaTemp = GetTargetTemperature();

            // Smanjena histereza za bržu reakciju (0.2°C umesto 0.5°C)
            if (prosecnaTemp < ciljnaTemp - 0.2 && !_heaterService.IsOn())
            {
                _heaterService.TurnOn();
                _devicesService.NotifyHeaterStateChange(true);
                _logger.LogEvent($"Peć uključena - Prosečna: {prosecnaTemp:F1}°C, Ciljna: {ciljnaTemp}°C");
            }
            else if (prosecnaTemp >= ciljnaTemp + 0.2 && _heaterService.IsOn())
            {
                _heaterService.TurnOff();
                _devicesService.NotifyHeaterStateChange(false);
                _logger.LogEvent($"Peć isključena - Prosečna: {prosecnaTemp:F1}°C, Ciljna: {ciljnaTemp}°C");
            }
        }
    }
}