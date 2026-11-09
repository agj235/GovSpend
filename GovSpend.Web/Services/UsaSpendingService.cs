using GovSpend.Web.DTOs;
using System.Net.Http.Json;

namespace GovSpend.Web.Services;

public class UsaSpendingService : ISpendingService
{
   private readonly HttpClient _httpClient;
   public UsaSpendingService(HttpClient httpClient)
   {
      _httpClient = httpClient;
   }
   public async Task<decimal> GetTotalSpendingAsync()
   {
      using HttpResponseMessage response = await _httpClient.GetAsync("api/v2/references/toptier_agencies/");

      response.EnsureSuccessStatusCode();

      string json = await response.Content.ReadAsStringAsync();

      Console.WriteLine($"Response JSON: {json}");
      
       // Log the response for debugging
      return 1500000m; // Return a dummy total spending value
   }

   public async Task<List<AgencyDto>> GetTopTierAgenciesAsync()
   {
      AgencyResponseDto? response = await _httpClient.GetFromJsonAsync<AgencyResponseDto>("api/v2/references/toptier_agencies/");
      
      foreach (var agency in response?.Results ?? new List<AgencyDto>())
      {
         Console.WriteLine(
            $"Agency: {agency.AgencyName} | " +
            $"Obligated: {agency.ObligatedAmount}"
         );
      }
      return response?.Results ?? new List<AgencyDto>();
   }
}