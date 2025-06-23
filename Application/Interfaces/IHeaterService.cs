using System;
using Domain;

namespace Application.Interfaces
{
    public interface IHeaterService
    {
        void TurnOn();
        void TurnOff();
        bool IsHeaterOn();
        DateTime? GetLastOnTime();

        // Ukupno vreme i potrošnja do poslednjeg gašenja grejača
        TimeSpan GetTotalWorkingTime();
        double GetTotalResourceUsed();

        // NOVE metode: ukupno vreme/potrošnja uključujući i tekuću sesiju
        TimeSpan GetTotalWorkingTimeLive();
        double GetTotalResourceUsedLive();

        void Reset();
    }
}
