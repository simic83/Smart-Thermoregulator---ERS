using System;

namespace Application.Interfaces
{
    public interface IRegulatorService
    {
        void SetDayNightRanges((int, int) day, (int, int) night);
        void SetDesiredTemperatures(double day, double night);
        void ProcessTemperature(string deviceId, double value);
        void SubscribeToDeviceEvents();
        void UnsubscribeFromDeviceEvents();
        string CurrentMode { get; }
    }
}
