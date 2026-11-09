using GovSpend.Web.DTOs;
namespace GovSpend.Web.Services;


public interface ISpendingService
{
    Task<decimal> GetTotalSpendingAsync();

    Task<List<AgencyDto>> GetTopTierAgenciesAsync();
}