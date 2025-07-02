using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{

    using global::Domain.Enums;

    public class Regulator
    {
        public string Id { get; set; }
        public RezimRada TrenutniRezim { get; set; }
        public int PocetakDnevnogRezima { get; set; }
        public int KrajDnevnogRezima { get; set; }
        public double TemperaturaDnevna { get; set; }
        public double TemperaturaNocna { get; set; }
        public List<TemperaturnoPocitavanje> PocitanjaTemperature { get; set; }

        public Regulator()
        {
            Id = "MainRegulator001";
            PocitanjaTemperature = new List<TemperaturnoPocitavanje>();
        }
    }

    public class TemperaturnoPocitavanje
    {
        public string DeviceId { get; set; }
        public double Temperatura { get; set; }
        public DateTime Vreme { get; set; }
    }
}