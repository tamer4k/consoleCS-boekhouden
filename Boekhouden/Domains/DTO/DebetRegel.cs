using System;

namespace Boekhouden.Domains.DTO
{
    public class DebetRegel : GrootboekRegel
    {
        public double Debet { get; set; }



        public DebetRegel(double debet, DateTime datum, int grootboekRekening, string grootboek) : base(datum, grootboekRekening, grootboek)
        {
            this.Debet = debet;
        }
        public DebetRegel()
        {

        }
        public void Prints()
        {
            Console.WriteLine(" Debet : {0} \n Datum : {1} \n GrootboekRekening : {2} \n Grootboek : {3} \n \n \n", Debet, Datum, GrootboekRekening, Grootboek);
            Console.WriteLine("================");
        }
    }
}
