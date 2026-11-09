using GovSpend.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GovSpend.Web.Data;

public class GovSpendDbContext : DbContext
{
    public GovSpendDbContext(DbContextOptions<GovSpendDbContext> options) : base(options)
    {
    }

    public DbSet<AgencyRecord> AgencyRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AgencyRecord>().HasIndex(a => a.FederalAgencyId).IsUnique();
    }
}