using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    public class Device
    {
        public string Id { get; }
        public double CurrentTemperature { get; private set; }
        public bool IsHeating { get; private set; }

        public event Action<string, double>? OnTemperatureChecked;

        private CancellationTokenSource? _cancellationTokenSource;
        private readonly object _lock = new object();

        public Device(string id, double initialTemperature = 20.0)
        {
            Id = id;
            CurrentTemperature = initialTemperature;
            IsHeating = false;
        }

        public void Start(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => DeviceLoop(_cancellationTokenSource.Token));
        }

        private async Task DeviceLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(3), token);
                lock (_lock)
                {
                    OnTemperatureChecked?.Invoke(Id, CurrentTemperature);
                }
            }
        }

        public void StartHeating()
        {
            lock (_lock)
            {
                if (!IsHeating)
                {
                    IsHeating = true;
                    if (_cancellationTokenSource != null)
                        Task.Run(() => HeatingLoop(_cancellationTokenSource.Token));
                }
            }
        }

        public void StopHeating()
        {
            lock (_lock)
            {
                IsHeating = false;
            }
        }

        private async Task HeatingLoop(CancellationToken token)
        {
            while (IsHeating && !token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(2), token);
                lock (_lock)
                {
                    if (IsHeating)
                        CurrentTemperature += 0.01;
                }
            }
        }

        public void SetTemperature(double temperature)
        {
            lock (_lock)
            {
                CurrentTemperature = temperature;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        public override string ToString()
        {
            return $"Oznaka: {Id}, Temp: {CurrentTemperature:F2}°C, Grejanje: {(IsHeating ? "DA" : "NE")}";
        }
    }
}
