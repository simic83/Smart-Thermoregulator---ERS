using System;

namespace Domain
{
    public class HeaterCycleInfo
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Duration { get; }
        public double ResourceUsed { get; }

        public HeaterCycleInfo(DateTime startTime, DateTime endTime, TimeSpan duration, double resourceUsed)
        {
            StartTime = startTime;
            EndTime = endTime;
            Duration = duration;
            ResourceUsed = resourceUsed;
        }

        public override string ToString()
        {
            return $"Početak: {StartTime:yyyy-MM-dd HH:mm:ss}, Kraj: {EndTime:yyyy-MM-dd HH:mm:ss}, Trajanje: {Duration.TotalMinutes:F1} min, Resurs: {ResourceUsed:F2} kWh";
        }
    }
}
