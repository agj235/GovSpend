using System.Text.Json.Serialization;

namespace GovSpend.Web.DTOs;
public class AgencyResponseDto
{
    [JsonPropertyName("results")]
    public List<AgencyDto> Results { get; set; } = new();
}