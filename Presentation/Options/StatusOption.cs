using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Presentation.Display;

namespace Presentation.Options
{

    public class StatusOption
    {
        private readonly IDevicesService _devicesService;
        private readonly IHeaterService _heaterService;
        private readonly IRegulatorService _regulatorService;

        public StatusOption(IDevicesService devicesService, IHeaterService heaterService, IRegulatorService regulatorService)
        {
            _devicesService = devicesService;
            _heaterService = heaterService;
            _regulatorService = regulatorService;
        }

        public void Execute()
        {
            var statusDisplay = new StatusDisplay(_devicesService, _heaterService, _regulatorService);
            statusDisplay.ShowLiveStatus();
        }
    }
}