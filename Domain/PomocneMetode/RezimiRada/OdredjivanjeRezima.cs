using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode.RezimiRada
{
    using global::Domain.Enums;

    public static class OdredjivanjeRezima
    {
        public static RezimRada OdrediRezim(int pocetakDana, int krajDana)
        {
            var trenutnoVreme = DateTime.Now.Hour;

            if (trenutnoVreme >= pocetakDana && trenutnoVreme < krajDana)
            {
                return RezimRada.Dnevni;
            }

            return RezimRada.Nocni;
        }
    }
}
