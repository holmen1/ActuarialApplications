using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

    public class ActuarialApplicationsRateContext : DbContext
    {
        public ActuarialApplicationsRateContext (DbContextOptions<ActuarialApplicationsRateContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF demands primary keys
            modelBuilder.Entity<ActuarialApplications.Models.Rate>(entity => {
                entity.HasKey(c => new { c.ValueDate, c.ParameterName, c.Duration });
            });
        }

        public DbSet<ActuarialApplications.Models.Rate> Rate { get; set; }
    }
