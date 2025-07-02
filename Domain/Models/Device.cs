using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Device
    {
        public string Id { get; set; }
        public double Temperatura { get; set; }
        public DateTime PoslednjeOcitavanje { get; set; }
        public bool JePecUkljucena { get; set; }

        public Device(string id)
        {
            Id = id;
            Temperatura = 20.0; // Pocetna temperatura
            PoslednjeOcitavanje = DateTime.Now;
            JePecUkljucena = false;
        }
    }
}