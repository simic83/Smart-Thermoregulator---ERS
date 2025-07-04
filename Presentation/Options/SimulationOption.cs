using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Presentation.Simulation;

namespace Presentation.Options
{

    public class SimulationOption
    {
        private readonly IDevicesService _devicesService;
        private readonly IHeaterService _heaterService;
        private readonly IRegulatorService _regulatorService;
        private static bool _simulationRunning = false;
        private static CancellationTokenSource _cancellationTokenSource;

        public SimulationOption(IDevicesService devicesService, IHeaterService heaterService, IRegulatorService regulatorService)
        {
            _devicesService = devicesService;
            _heaterService = heaterService;
            _regulatorService = regulatorService;
        }

        public async Task Execute()
        {
            if (!_simulationRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n▶ Pokretanje simulacije...");
                Console.ResetColor();

                _simulationRunning = true;
                _cancellationTokenSource = new CancellationTokenSource();
                _regulatorService.StartRegulation();

                var simulator = new SimulationRunner(_devicesService, _heaterService, _regulatorService);
                _ = Task.Run(() => simulator.RunSimulation(_cancellationTokenSource.Token));

                Thread.Sleep(1500);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n■ Zaustavljanje simulacije...");
                Console.ResetColor();

                _simulationRunning = false;
                _cancellationTokenSource?.Cancel();

                Thread.Sleep(1500);
            }
        }
    }
}