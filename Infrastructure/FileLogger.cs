using System;
using System.IO;

namespace Infrastructure
{
    public class FileLogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public FileLogger(string filePath = "log.txt")
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            string formatted = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            lock (_lock)
            {
                File.AppendAllText(_filePath, formatted + Environment.NewLine);
            }
        }

        public void Log(string level, string message)
        {
            string formatted = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToUpper()}] {message}";
            lock (_lock)
            {
                File.AppendAllText(_filePath, formatted + Environment.NewLine);
            }
        }
    }
}
