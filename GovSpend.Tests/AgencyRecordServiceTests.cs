using GovSpend.Web.Data;
using Microsoft.EntityFrameworkCore;
using GovSpend.Web.DTOs;
using GovSpend.Web.Services;
using Microsoft.Data.Sqlite;
using Xunit;
using GovSpend.Web.Models;

namespace GovSpend.Tests;

public class AgencyRecordServiceTests
{
    
private static async Task<(
    GovSpendDbContext Context,
    SqliteConnection Connection
)> CreateTestContextAsync()
{
    var connection = new SqliteConnection("DataSource=:memory:");
    await connection.OpenAsync();

    var options = new DbContextOptionsBuilder<GovSpendDbContext>()
        .UseSqlite(connection)
        .Options;

    var context = new GovSpendDbContext(options);

    await context.Database.EnsureCreatedAsync();

    return (context, connection);
}

    [Fact]
    public async Task SaveAgencyAsync_NewAgency_ReturnsTrue()
    {
        //Arrange - Set up the test database, service, and sample agency
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GovSpendDbContext>()
            .UseSqlite(connection)
            .Options;

            await using var context = new GovSpendDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new AgencyRecordService(context);

        var agency = new AgencyDto
        {
            AgencyId = 123,
            AgencyName = "Test Federal Agency",
            ToptierCode = "123",
            ObligatedAmount = 5000000m
        };

        //Act - execute SaveAgencyAsync()
        bool result = await service.SaveAgencyAsync(agency);

        //Assert - Verify the method returned true and save one record.
        Assert.True(result);
        Assert.Equal(1, await context.AgencyRecords.CountAsync());
    }

    [Fact]
    public async Task SaveAgencyAsync_DuplicateAgency_ReturnsFalse()
    {
        //Arrange 
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GovSpendDbContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new GovSpendDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new AgencyRecordService(context);
        var agency = new AgencyDto
        {
            AgencyId = 456,
            AgencyName = "Department of Testing",
            ToptierCode = "456",
            ObligatedAmount = 7000000m
        };

        //Act
        bool firstSaveResult = await service.SaveAgencyAsync(agency);
        bool secondSaveResult = await service.SaveAgencyAsync(agency);

        //Assert
        Assert.True(firstSaveResult);
        Assert.False(secondSaveResult);
        Assert.Equal(1, await context.AgencyRecords.CountAsync());
    }

    [Fact]
    public async Task UpdateAgencyAsync_ExistingAgency_ReturnsTrue()
    {
        //Arrange 
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GovSpendDbContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new GovSpendDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new AgencyRecordService(context);
        var agency = new AgencyRecord
        {
            FederalAgencyId = 798,
            Name = "Original Department",
            AgencyCode = "798",
            ObligatedAmount = 2000000m
        };
        context.AgencyRecords.Add(agency);
        await context.SaveChangesAsync();

        //Act
        bool result = await service.UpdateAgencyAsync(agency.Id, "Updated Agency");
    

        //Assert
        Assert.True(result);
        var updatedAgency = await context.AgencyRecords
        .AsNoTracking()
        .FirstAsync(a => a.Id == agency.Id);

        Assert.Equal("Updated Agency", updatedAgency.Name);
    }

    [Fact]
    public async Task DeleteAgencyAsync_ExistingAgency_ReturnsTrue()
    {
        //Arrange 
        using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GovSpendDbContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new GovSpendDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new AgencyRecordService(context);
        var agency = new AgencyRecord
        {
            FederalAgencyId = 321,
            Name = "Department to Delete",
            AgencyCode = "321",
            ObligatedAmount = 3000000m
        };
        context.AgencyRecords.Add(agency);
        await context.SaveChangesAsync();

        //Act
        bool result = await service.DeleteAgencyAsync(agency.Id);

        //Assert
        Assert.True(result);
        Assert.Equal(0, await context.AgencyRecords.CountAsync());
    }

    

[Fact]
public async Task DeleteAgencyAsync_NonexistentAgency_ReturnsFalse()
{
    // Arrange
    var (context, connection) = await CreateTestContextAsync();

    await using var db = context;
    await using var sqliteConnection = connection;

    var service = new AgencyRecordService(context);

    // Act
    bool result = await service.DeleteAgencyAsync(99999);

    // Assert
    Assert.False(result);
    Assert.Empty(await context.AgencyRecords.ToListAsync());
}



[Theory]
[InlineData("")]
[InlineData("   ")]
public async Task UpdateAgencyAsync_InvalidName_ReturnsFalse(string name)
{
    // Arrange
    using var connection = new SqliteConnection("DataSource=:memory:");
    await connection.OpenAsync();

    var options = new DbContextOptionsBuilder<GovSpendDbContext>()
        .UseSqlite(connection)
        .Options;

    await using var context = new GovSpendDbContext(options);
    await context.Database.EnsureCreatedAsync();

    var service = new AgencyRecordService(context);

    var agency = new AgencyRecord
    {
        FederalAgencyId = 1234,
        Name = "Original Agency",
        AgencyCode = "1234",
        ObligatedAmount = 1000000m
    };

    context.AgencyRecords.Add(agency);
    await context.SaveChangesAsync();

    // Act
    bool result = await service.UpdateAgencyAsync(agency.Id, name);

    // Assert
    Assert.False(result);

    var savedAgency = await context.AgencyRecords
        .AsNoTracking()
        .FirstAsync(a => a.Id == agency.Id);

    Assert.Equal("Original Agency", savedAgency.Name);
}


}