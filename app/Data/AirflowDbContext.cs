using ActuarialApplications.Models;
using Microsoft.EntityFrameworkCore;

public class AirflowDbContext : DbContext
{
    public AirflowDbContext(DbContextOptions<AirflowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Swap> Swap { get; set; }
    public DbSet<RiskFreeRate> RiskFreeRates { get; set; }
    public DbSet<RiskFreeRateData> RiskFreeRateData { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<CashFlow> CashFlows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //EF demands primary keys
        modelBuilder.Entity<Swap>(entity =>
        {
            entity.HasKey(s => new { s.ValueDate, Currency = s.Id, s.Tenor });
            entity.ToTable("swap", "holmen");
        });

        modelBuilder.Entity<RiskFreeRate>(entity => { entity.ToTable("rate", "holmen"); });

        //EF demands primary keys
        modelBuilder.Entity<RiskFreeRateData>(entity =>
        {
            entity.HasKey(r => new { r.ProjectionId, r.Maturity });
            entity.ToTable("rate_data", "holmen");
        });

        //EF demands primary keys
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(c => c.ContractNo);
            entity.ToTable("contract", "holmen");
        });

        //EF demands primary keys
        modelBuilder.Entity<CashFlow>(entity =>
        {
            entity.HasKey(c => new { c.ValueDate, c.ContractNo, c.Month });
            entity.ToTable("cashflow", "holmen");
        });
    }
}