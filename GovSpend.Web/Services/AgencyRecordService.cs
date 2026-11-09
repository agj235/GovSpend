using GovSpend.Web.Models;
using GovSpend.Web.Data;
using Microsoft.EntityFrameworkCore;
using GovSpend.Web.DTOs;

namespace GovSpend.Web.Services;

public class AgencyRecordService : IAgencyRecordService
{
    private readonly GovSpendDbContext _context;

    public AgencyRecordService(GovSpendDbContext context)
    {
        _context = context;
    }

    public async Task<List<AgencyRecord>> GetSavedAgenciesAsync()
    {
        return await _context.AgencyRecords.AsNoTracking().ToListAsync();
    }

    public async Task<bool> SaveAgencyAsync(AgencyDto agency)
    {
        bool exists = await _context.AgencyRecords.AnyAsync(a => a.FederalAgencyId == agency.AgencyId);
        if (exists)
        {
            return false; // Agency already exists
        }

        var record = new AgencyRecord
        {
            FederalAgencyId = agency.AgencyId,
            Name = agency.AgencyName,
            AgencyCode = agency.ToptierCode,
            FiscalYear = agency.ActiveFiscalYear,
            ObligatedAmount = agency.ObligatedAmount
        };

        _context.AgencyRecords.Add(record);

        try
        {
            await _context.SaveChangesAsync();
            return true; // Successfully saved
        }
        catch (DbUpdateException ex) when(
            ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx &&
            (sqlEx.Number == 2627 || sqlEx.Number == 2601)) //
        
        {
            // Log the error (uncomment ex variable name and write a log.)
            return false; // Error occurred while saving
        }
    }

    public async Task<bool> UpdateAgencyAsync(int id, string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            return false; // Invalid name
        }
        var agency = await _context.AgencyRecords.FindAsync(id);

        if (agency is null)
        {
            return false; // Agency not found

        }
        agency.Name = name.Trim();
        await _context.SaveChangesAsync();
        return true; // Successfully updated
    }

    public async Task<bool> DeleteAgencyAsync(int id)
    {
        var agency = await _context.AgencyRecords.FindAsync(id);
        if (agency is null)
        {
            return false; // Agency not found
        }

        _context.AgencyRecords.Remove(agency);
        await _context.SaveChangesAsync();
        return true; // Successfully deleted
    }
}