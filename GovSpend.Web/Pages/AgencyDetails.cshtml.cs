using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GovSpend.Web.DTOs;
using GovSpend.Web.Services;

namespace GovSpend.Web.Pages;

    public class AgencyDetailsModel : PageModel
    {
        private readonly ISpendingService _spendingService;
        private readonly IAgencyRecordService _agencyRecordService;
        public bool DemoMode {get; } 

        public decimal ObligationPercentage {get; set;}
        public AgencyDetailsModel(ISpendingService spendingService, IAgencyRecordService agencyRecordService, IConfiguration configuration)
    {
        _spendingService = spendingService;
        _agencyRecordService = agencyRecordService;
        DemoMode = configuration.GetValue<bool>("DemoMode");
    }

    public AgencyDto? Agency {get; set;}

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var agencies = await _spendingService.GetTopTierAgenciesAsync();

        Agency = agencies.FirstOrDefault(a => a.AgencyId == id);

        if(Agency is null)
        {
            return NotFound();

        }

        ObligationPercentage = Agency.BudgetAuthorityAmount > 0
        ? Agency.ObligatedAmount / Agency.BudgetAuthorityAmount * 100
        : 0;

        return Page();
    }

    
public async Task<IActionResult> OnPostSaveAsync(int id)
{

        if (DemoMode)
        {
            return StatusCode(403);
        }
    var agencies = await _spendingService.GetTopTierAgenciesAsync();

    var agency = agencies.FirstOrDefault(
        a => a.AgencyId == id
    );

    if (agency is null)
    {
        return NotFound();
    }

    bool saved = await _agencyRecordService.SaveAgencyAsync(agency);

    TempData["Message"] = saved
        ? "Agency saved successfully!"
        : "This agency is already saved.";

    return RedirectToPage(new { id });
}

    
}
