using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GovSpend.Web.Models;
using GovSpend.Web.Services;
using GovSpend.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GovSpend.Web.Pages;


public class IndexModel : PageModel
{   
    public decimal TotalSpending {get; set;}

    public int TotalFederalAgencies {get; set;}
    
    public decimal TopAgencyShare {get; set;}
    public List<Agency> Agencies { get; set; } = new();

    public List<AgencyDto> FederalAgencies { get; set; } = new();
    public List<AgencyRecord> SavedAgencies { get; set; } = new();

    public List<AgencyDto> TopFiveAgencies {get; set;} = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    private readonly ISpendingService _spendingService;
    private readonly IAgencyRecordService _agencyRecordService;
    public decimal TotalFederalObligations { get; set; }
    public string TopAgencyName {get; set; } = "N/A";

    public string? ErrorMessage {get; set;}

    public bool DemoMode {get; }

    public IndexModel(ISpendingService spendingService, IAgencyRecordService agencyRecordService, IConfiguration configuration)
    {
        _spendingService = spendingService;
        _agencyRecordService = agencyRecordService;

        DemoMode = configuration.GetValue<bool>("DemoMode");
    }
        
        public async Task<ActionResult> OnPostSaveAgencyAsync(int agencyId)
    {
        if (DemoMode)
        {
            return StatusCode(403);
        }

        
        var agencies = await _spendingService.GetTopTierAgenciesAsync();
        var agency = agencies.FirstOrDefault(a => a.AgencyId == agencyId);
        if (agency is null)
        {
            return NotFound();
        }
        await _agencyRecordService.SaveAgencyAsync(agency);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAgencyAsync(int id)
    {
        if (DemoMode)
        {
            return StatusCode(403);
        }
        bool deleted = await _agencyRecordService.DeleteAgencyAsync(id);
        if (!deleted)
        {
            return NotFound("Agency not found.");
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAgencyAsync(int id, string name)
    {
        if (DemoMode)
        {
            return StatusCode(403);
        }

        bool updated = await _agencyRecordService.UpdateAgencyAsync(id, name);

        if (!updated)
        {
            return BadRequest("Agency not found or invalid name.");
        }
        return RedirectToPage();
    }
    
    public async Task OnGetAsync()
    {
        SavedAgencies = await _agencyRecordService.GetSavedAgenciesAsync();
        
        try{
        FederalAgencies = 
        await _spendingService.GetTopTierAgenciesAsync();
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Federal spending data is temporarily unavailable. Please try again later.";
            FederalAgencies = new List<AgencyDto>();
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "The federal spending data request times out. Please try again later.";
            FederalAgencies = new List<AgencyDto>();
        }

        TotalFederalObligations = FederalAgencies.Sum(a => a.ObligatedAmount);

        TotalFederalAgencies = FederalAgencies.Count;

        TopFiveAgencies = FederalAgencies
        .OrderByDescending(a => a.ObligatedAmount)
        .Take(5)
        .ToList();

        TopAgencyName = TopFiveAgencies
        .FirstOrDefault()?.AgencyName ?? "N/A";

        TopAgencyShare = TotalFederalObligations > 0
        ? TopFiveAgencies.FirstOrDefault()?.ObligatedAmount
            / TotalFederalObligations * 100 ?? 0
            : 0;

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            FederalAgencies = FederalAgencies
                .Where(a => a.AgencyName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        TotalSpending = await _spendingService.GetTotalSpendingAsync();
        
        

    }
    
}
