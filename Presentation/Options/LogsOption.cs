using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Display;

namespace Presentation.Options
{

    public class LogsOption
    {
        public void Execute()
        {
            var logsDisplay = new LogsDisplay();
            logsDisplay.ShowLogs();
        }
    }
}
