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
    }
    
        public DbSet<ActuarialApplications.Models.Swap> Swap { get; set; }
    }