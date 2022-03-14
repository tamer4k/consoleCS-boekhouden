namespace Boekhouden.Domains.DTO
{
    public class DagTotaal
    {
        public DebetRegel GeldOntvangen { get; set; }
        public CreditRegel OmzetBtwLaag { get; set; }
        public CreditRegel OmzetBtwHoog { get; set; }
        public CreditRegel BtwLaag { get; set; }
        public CreditRegel BtwHoog { get; set; }
        public CreditRegel OmzetGeenBTW { get; set; }


        public DagTotaal()
        {
            GeldOntvangen = new DebetRegel();
            OmzetBtwLaag = new CreditRegel();
            OmzetBtwHoog = new CreditRegel();
            BtwLaag = new CreditRegel();
            BtwHoog = new CreditRegel();
            OmzetGeenBTW = new CreditRegel();

        }
    }
}
