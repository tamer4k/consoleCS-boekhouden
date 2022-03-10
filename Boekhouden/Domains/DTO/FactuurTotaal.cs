namespace Boekhouden.Domains.DTO
{
    public class FactuurTotaal
    {
        public DebetRegel GeldOntvangen { get; set; }
        public CreditRegel OmzetBtwLaag { get; set; }
        public CreditRegel OmzetBtwHoog { get; set; }
        public CreditRegel BtwLaag { get; set; }
        public CreditRegel BtwHoog { get; set; }
    }
}
