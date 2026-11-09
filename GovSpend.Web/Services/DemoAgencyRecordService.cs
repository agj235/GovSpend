
using GovSpend.Web.DTOs;
using GovSpend.Web.Models;

namespace GovSpend.Web.Services;

public class DemoAgencyRecordService : IAgencyRecordService
{
    public Task<List<AgencyRecord>> GetSavedAgenciesAsync()
    {
        return Task.FromResult(new List<AgencyRecord>());
    }

    public Task<bool> SaveAgencyAsync(AgencyDto agency)
    {
        return Task.FromResult(false);
    }

    public Task<bool> UpdateAgencyAsync(int id, string name)
    {
        return Task.FromResult(false);
    }

    public Task<bool> DeleteAgencyAsync(int id)
    {
        return Task.FromResult(false);
    }
}
