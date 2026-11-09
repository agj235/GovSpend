using GovSpend.Web.Models;
using GovSpend.Web.DTOs;
namespace GovSpend.Web.Services;
public interface IAgencyRecordService
{
    Task<List<AgencyRecord>> GetSavedAgenciesAsync();
    Task<bool> SaveAgencyAsync(AgencyDto agency);
    Task<bool> UpdateAgencyAsync(int id, string name);
    Task<bool> DeleteAgencyAsync(int id);
}