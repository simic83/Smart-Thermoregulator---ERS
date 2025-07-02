using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode.Temperature
{
    using global::Domain.Models;

    public static class RacunanjeProsecneTemperature
    {
        public static double IzracunajProsek(List<TemperaturnoPocitavanje> pocitanja)
        {
            if (pocitanja == null || pocitanja.Count == 0)
                return 20.0; // Default temperatura

            // Uzimamo samo najnovija očitavanja (poslednja 4)
            var skorijaPocitanja = pocitanja
                .OrderByDescending(p => p.Vreme)
                .Take(4)
                .ToList();

            if (skorijaPocitanja.Count == 0)
                return 20.0;

            // Grupisanje po uređajima i uzimanje najnovijeg očitavanja po uređaju
            var najnovijePoUredjaju = skorijaPocitanja
                .GroupBy(p => p.DeviceId)
                .Select(g => g.OrderByDescending(p => p.Vreme).First())
                .ToList();

            return najnovijePoUredjaju.Average(p => p.Temperatura);
        }
    }
}