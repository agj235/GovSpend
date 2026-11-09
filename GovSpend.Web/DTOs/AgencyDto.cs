using System.Text.Json.Serialization;

namespace GovSpend.Web.DTOs;
public class AgencyDto
{
    [JsonPropertyName("agency_id")]
    public int AgencyId { get; set; }

    [JsonPropertyName("toptier_code")]
    public string ToptierCode { get; set; } = string.Empty;

    [JsonPropertyName("abbreviation")]
    public string? Abbreviation { get; set; }

    [JsonPropertyName("agency_name")]
    public string AgencyName { get; set; } = string.Empty;

    [JsonPropertyName("active_fy")]
    public string? ActiveFiscalYear { get; set; }
    [JsonPropertyName("outlay_amount")]
    public decimal OutlayAmount { get; set; }
    [JsonPropertyName("obligated_amount")]
    public decimal ObligatedAmount { get; set; }
    [JsonPropertyName("budget_authority_amount")]
    public decimal BudgetAuthorityAmount { get; set; }

    [JsonPropertyName("agency_slug")]
    public string AgencySlug { get; set; } = string.Empty;


    
}