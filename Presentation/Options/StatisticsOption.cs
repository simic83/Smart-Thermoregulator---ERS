using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Presentation.Display;

namespace Presentation.Options
{

    public class StatisticsOption
    {
        private readonly IHeaterService _heaterService;

        public StatisticsOption(IHeaterService heaterService)
        {
            _heaterService = heaterService;
        }

        public void Execute()
        {
            var statsDisplay = new StatisticsDisplay(_heaterService);
            statsDisplay.ShowStatistics();
        }
    }
}
