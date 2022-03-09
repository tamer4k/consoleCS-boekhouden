using System;

namespace Boekhouden.Domains.DTO
{
    public class GrootboekRegel
    {
        public DateTime Datum { get; set; }
        public int GrootboekRekening { get; set; }
        public string Grootboek { get; set; }


        public GrootboekRegel(DateTime datum, int grootboekRekening, string grootboek)
        {
            this.Datum = datum;
            this.GrootboekRekening = grootboekRekening;
            this.Grootboek = grootboek;

        }
    }
}
