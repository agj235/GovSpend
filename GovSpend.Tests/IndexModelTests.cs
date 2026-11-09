using GovSpend.Web.DTOs;
using GovSpend.Web.Services;
using Moq;
using Xunit;
using GovSpend.Web.Pages;
using GovSpend.Web.Models;

namespace GovSpend.Tests;

public class IndexModelTests
{
    [Fact]
    public async Task MockSpendingService_ReturnsTestAgencies()
    {
        //Arrange
        var mockSpendingService = new Mock<ISpendingService>();

        var testAgencies = new List<AgencyDto>
        {
            new AgencyDto { AgencyId = 100, AgencyName = "Test Agency 1", ToptierCode = "100"}
        };

        mockSpendingService
        .Setup(s => s.GetTopTierAgenciesAsync())
        .ReturnsAsync(testAgencies);

        //Act
        var result = await mockSpendingService.Object.GetTopTierAgenciesAsync();

        //Assert
        Assert.Single(result);
        Assert.Equal("Test Agency 1", result[0].AgencyName);
    }

    [Fact]
    public async Task OnGetAsync_LoadsFederalAgencies()
    {
        var mockSpendingService = new Mock<ISpendingService>();
        var mockAgencyService = new Mock<IAgencyRecordService>();

        var testAgencies = new List<AgencyDto>
        {
            new AgencyDto 
            { AgencyId = 100, AgencyName = "Department of Test", ToptierCode = "100"}
        };

        mockSpendingService.Setup(s => s.GetTopTierAgenciesAsync()).ReturnsAsync(testAgencies);

        mockAgencyService.Setup(s => s.GetSavedAgenciesAsync()).ReturnsAsync(new List<AgencyRecord>());

        var pageModel = new IndexModel(mockSpendingService.Object, mockAgencyService.Object);

        await pageModel.OnGetAsync();

        Assert.Single(pageModel.FederalAgencies);
    }

    [Fact]
    public async Task OnGetAsync_SearchTerm_FiltersAgencies()
    {
        var mockSpendingService = new Mock<ISpendingService>();
        var mockAgencyService = new Mock<IAgencyRecordService>();

        var agencies = new List<AgencyDto>
        {
            new AgencyDto { AgencyId = 100, AgencyName = "Department of Justice", ToptierCode = "100" },
            new AgencyDto { AgencyId = 101, AgencyName = "Department of Defense", ToptierCode = "101" }
        };

        mockSpendingService.Setup(s => s.GetTopTierAgenciesAsync()).ReturnsAsync(agencies);

        mockAgencyService.Setup(s => s.GetSavedAgenciesAsync()).ReturnsAsync(new List<AgencyRecord>());

        var pageModel = new IndexModel(mockSpendingService.Object, mockAgencyService.Object)
        {
            SearchTerm = "Justice"
        };
        await pageModel.OnGetAsync();

        Assert.Single(pageModel.FederalAgencies);

        Assert.Equal("Department of Justice", pageModel.FederalAgencies[0].AgencyName);
    }

    
[Fact]
public async Task OnGetAsync_CallsServicesOnce()
{
    // Arrange
    var mockSpendingService = new Mock<ISpendingService>();
    var mockAgencyService = new Mock<IAgencyRecordService>();

    mockSpendingService
        .Setup(s => s.GetTopTierAgenciesAsync())
        .ReturnsAsync(new List<AgencyDto>());

    mockAgencyService
        .Setup(s => s.GetSavedAgenciesAsync())
        .ReturnsAsync(new List<AgencyRecord>());

    var pageModel = new IndexModel(
        mockSpendingService.Object,
        mockAgencyService.Object
    );

    // Act
    await pageModel.OnGetAsync();

    // Assert
    mockSpendingService.Verify(
        s => s.GetTopTierAgenciesAsync(),
        Times.Once()
    );

    mockAgencyService.Verify(
        s => s.GetSavedAgenciesAsync(),
        Times.Once()
    );
}

}