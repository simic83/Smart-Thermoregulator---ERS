using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IHeaterService
    {
        void TurnOn();
        void TurnOff();
        bool IsOn();
        double GetTotalWorkingHours();
        double GetResourceConsumption();
    }
}