using System;

namespace Boekhouden.Domains.DTO
{
    public class CreditRegel : GrootboekRegel
    {
        public double Credit { get; set; }


        public CreditRegel(double credit, DateTime datum, int grootboekRekening, string grootboek) : base(datum, grootboekRekening, grootboek)
        {
            this.Credit = credit;
        }
        public CreditRegel()
        {

        }
        public void Prints()
        {
            Console.WriteLine(" Credit : {0} \n Datum : {1} \n GrootboekRekening : {2} \n Grootboek : {3} \n \n \n", Credit, Datum, GrootboekRekening, Grootboek);
            Console.WriteLine("================");
        }
    }
}
