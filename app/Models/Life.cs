using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Models
{
    public class Contract
    {
        public int ContractNo { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; } // M or F
        public int VestingAge { get; set; }
        public double GuaranteeBenefit { get; set; }
        public int PayPeriod { get; set; }
        public string Table { get; set; } // AP or APG

        public static class SeedContractData
        {
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new LocalLifeDbContext(
                           serviceProvider.GetRequiredService<
                               DbContextOptions<LocalLifeDbContext>>()))
                {
                    if (context.Contracts.Any())
                    {
                        return; // DB has been seeded
                    }

                    context.Contracts.AddRange(
                        new Contract
                        {
                            ContractNo = 123,
                            ValueDate = DateTime.Parse("2020-01-01"),
                            BirthDate = DateTime.Parse("1974-01-01"),
                            Sex = "M",
                            VestingAge = 65,
                            GuaranteeBenefit = 1000,
                            PayPeriod = 10,
                            Table = "AP"
                        },
                        new Contract
                        {
                            ContractNo = 124,
                            ValueDate = DateTime.Parse("2020-01-01"),
                            BirthDate = DateTime.Parse("1974-01-01"),
                            Sex = "F",
                            VestingAge = 65,
                            GuaranteeBenefit = 1000,
                            PayPeriod = 10,
                            Table = "AP"
                        },
                        new Contract
                        {
                            ContractNo = 125,
                            ValueDate = DateTime.Parse("2020-01-01"),
                            BirthDate = DateTime.Parse("1974-01-01"),
                            Sex = "M",
                            VestingAge = 65,
                            GuaranteeBenefit = 1000,
                            PayPeriod = 10,
                            Table = "APG"
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
