namespace GovSpend.Web.Models
{
    public class AgencyRecord
    {
        public int Id { get; set; }
        public int FederalAgencyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AgencyCode { get; set; }
        public string? FiscalYear { get; set; }
        public decimal ObligatedAmount { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
        public bool DemoMode {get; } 
        

    }
}