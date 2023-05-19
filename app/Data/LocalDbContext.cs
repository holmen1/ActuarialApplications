using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

    public class LocalDbContext : DbContext
    {
        public LocalDbContext (DbContextOptions<LocalDbContext> options)
            : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //EF demands primary keys
        modelBuilder.Entity<ActuarialApplications.Models.Swap>(entity => {
            entity.HasKey(s => new { s.ValueDate, s.Currency, s.Tenor });
        });

        //EF demands primary keys
        modelBuilder.Entity<ActuarialApplications.Models.RiskFreeRateData>(entity => {
            entity.HasKey(r => new { r.ProjectionId, r.Maturity });
        });
    }
    
        public DbSet<Swap> Swap { get; set; }
        public DbSet<RiskFreeRate> RiskFreeRates { get; set; }
        public DbSet<RiskFreeRateData> RiskFreeRateData { get; set; }
    }
