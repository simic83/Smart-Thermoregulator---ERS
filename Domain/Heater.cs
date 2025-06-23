using System;

namespace Domain
{
    public class Heater
    {
        // Da li je grejač trenutno uključen
        public bool IsOn { get; private set; }

        // Vreme kada je grejač poslednji put upaljen
        public DateTime? LastOnTime { get; private set; }

        // Ukupno vreme koje je grejač proveo uključen (zatvorene sesije)
        public TimeSpan TotalWorkingTime { get; private set; }

        // Ukupna količina resursa koju je grejač potrošio (zatvorene sesije)
        public double TotalResourceUsed { get; private set; }

        public Heater()
        {
            IsOn = false;
            LastOnTime = null;
            TotalWorkingTime = TimeSpan.Zero;
            TotalResourceUsed = 0.0;
        }

        // Poziva se kada regulator šalje komandu za paljenje
        public void TurnOn()
        {
            if (!IsOn)
            {
                IsOn = true;
                LastOnTime = DateTime.Now;
            }
        }

        // Poziva se kada regulator šalje komandu za gašenje
        public void TurnOff()
        {
            if (IsOn)
            {
                IsOn = false;

                if (LastOnTime.HasValue)
                {
                    TimeSpan workingThisSession = DateTime.Now - LastOnTime.Value;
                    TotalWorkingTime += workingThisSession;

                    const double GREJAC_SNAGA_W = 1000; // W
                    double resourceThisSession = GREJAC_SNAGA_W * workingThisSession.TotalMinutes / 60.0;
                    TotalResourceUsed += resourceThisSession;
                }

                LastOnTime = null;
            }
        }


        // Vraća ukupno radno vreme uključujući i trenutnu (otvorenu) sesiju
        public TimeSpan GetTotalWorkingTimeLive()
        {
            if (IsOn && LastOnTime.HasValue)
                return TotalWorkingTime + (DateTime.Now - LastOnTime.Value);
            else
                return TotalWorkingTime;
        }

        // Vraća ukupnu potrošnju resursa uključujući i trenutnu (otvorenu) sesiju
        public double GetTotalResourceUsedLive()
        {
            const double GREJAC_SNAGA_W = 1000; // 1000W = 1kW

            if (IsOn && LastOnTime.HasValue)
            {
                var duration = DateTime.Now - LastOnTime.Value;
                double ukupnaMinuta = duration.TotalMinutes;
                // Potrošnja u Wh (vati-sati)
                double potrosnjaWh = TotalResourceUsed + (GREJAC_SNAGA_W * ukupnaMinuta / 60.0);
                return potrosnjaWh;
            }
            else
            {
                return TotalResourceUsed;
            }
        }

        public HeaterCycleInfo? TurnOffAndGetCycleInfo()
        {
            if (IsOn && LastOnTime.HasValue)
            {
                IsOn = false;
                var endTime = DateTime.Now;
                var duration = endTime - LastOnTime.Value;
                var resource = duration.TotalMinutes * 0.1;

                // Update totals
                TotalWorkingTime += duration;
                TotalResourceUsed += resource;

                var cycle = new HeaterCycleInfo(LastOnTime.Value, endTime, duration, resource);
                LastOnTime = null;
                return cycle;
            }
            return null;
        }

        // Ručno resetovanje
        public void Reset()
        {
            IsOn = false;
            LastOnTime = null;
            TotalWorkingTime = TimeSpan.Zero;
            TotalResourceUsed = 0.0;
        }
    }
}
