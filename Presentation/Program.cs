using System;
using System.Collections.Generic;
using Application.Services;
using Domain;
using Infrastructure;
using Presentation.Screens;

class Program
{
    static void Main(string[] args)
    {
        // Inicijalizacija uređaja
        var devices = new List<Device>
        {
            new Device("1", 21.2),
            new Device("2", 20.7),
            new Device("3", 19.9),
            new Device("4", 21.0)
        };

        var heater = new Heater();

        // Inicijalizacija servisa
        var fileLogger = new FileLogger();
        var heaterCycle = new HeaterCycleLogger();
        var deviceService = new DeviceService(devices);
        var heaterService = new HeaterService(heater, heaterCycle, fileLogger);
        var regulatorService = new RegulatorService(deviceService, heaterService, fileLogger);
        
        // Poziv glavnog menija i prosleđivanje svih servisa
        GlavniMeniScreen.Prikazi(deviceService, heaterService, regulatorService, fileLogger, heaterCycle);
    }
}
