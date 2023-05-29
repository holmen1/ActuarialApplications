using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

public class LocalLifeDbContext : DbContext
{
    public LocalLifeDbContext(DbContextOptions<LocalLifeDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //EF demands primary keys
        modelBuilder.Entity<Contract>(entity => { entity.HasKey(c => new { c.ValueDate, c.ContractNo }); });
    }

    public DbSet<Contract> Contracts { get; set; }
}