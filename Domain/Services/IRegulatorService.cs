using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    using global::Domain.Enums;

    public interface IRegulatorService
    {
        void ConfigureWorkMode(int dayStart, int dayEnd, double dayTemp, double nightTemp);
        void ProcessTemperatureReading(string deviceId, double temperature);
        RezimRada GetCurrentMode();
        double GetTargetTemperature();
        void StartRegulation();
    }
}