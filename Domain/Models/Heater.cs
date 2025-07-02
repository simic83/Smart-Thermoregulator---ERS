using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Heater
    {
        public string Id { get; set; }
        public bool JeUkljucen { get; set; }
        public DateTime? VremePocetkaRada { get; set; }
        public List<RadniPeriod> RadniPeriodi { get; set; }

        public Heater()
        {
            Id = "CentralnaHeater001";
            JeUkljucen = false;
            RadniPeriodi = new List<RadniPeriod>();
        }
    }

    public class RadniPeriod
    {
        public DateTime VremePocetka { get; set; }
        public DateTime? VremeKraja { get; set; }
        public double TrajanjeSati { get; set; }
        public double PotroseniResursi { get; set; }
    }
}