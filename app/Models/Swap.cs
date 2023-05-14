using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Models
{
    // Enum class for currencies
    public enum Currency { USD, EUR, SEK, NOK, DKK }

    public class Swap
    {
        [DataType(DataType.Date)]
        public DateTime ValueDate { get; set; }
        public string Currency { get; set; }
        public int Tenor { get; set; }
        public double? SettlementFreq { get; set; }
        public double? Value { get; set; }
    }

    public static class SeedSwapData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LocalDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<LocalDbContext>>()))
            {
                // Look for any swaps
                if (context.Swap.Any())
                {
                    return;   // DB has been seeded
                }

                context.Swap.AddRange(
                    new Swap
                    {
                        ValueDate = DateTime.Parse("2023-03-31"),
                        Currency = "SEK",
                        Tenor = 2,
                        SettlementFreq = 1,
                        Value = 0.03495
                    },
                    new Swap
                    {
                        ValueDate = DateTime.Parse("2023-03-31"),
                        Currency = "SEK",
                        Tenor = 3,
                        SettlementFreq = 1,
                        Value = 0.0324
                    },
                    new Swap
                    {
                        ValueDate = DateTime.Parse("2023-03-31"),
                        Currency = "SEK",
                        Tenor = 5,
                        SettlementFreq = 1,
                        Value = 0.0298
                    },
                    new Swap
                    {
                        ValueDate = DateTime.Parse("2023-03-31"),
                        Currency = "SEK",
                        Tenor = 10,
                        SettlementFreq = 1,
                        Value = 0.02855
                    }
                );
                context.SaveChanges();
            }
        }
    }
}