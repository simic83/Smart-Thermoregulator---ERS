using System;
using Domain;

namespace Infrastructure
{
    public class HeaterCycleLogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public HeaterCycleLogger(string filePath = "grejac-log.txt")
        {
            _filePath = filePath;
        }

        public void LogCycle(HeaterCycleInfo cycle)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {cycle}";
            lock (_lock)
            {
                System.IO.File.AppendAllText(_filePath, line + Environment.NewLine);
            }
        }
    }
}
