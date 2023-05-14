using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Models
{
    public class SwParameters
    {
        [JsonPropertyName("par_rates")]
        public List<double> ParRates { get; set; }

        [JsonPropertyName("par_maturities")]
        public List<int> ParMaturities { get; set; }

        [JsonPropertyName("projection")]
        public List<int> Projection { get; set; }

        [JsonPropertyName("ufr")]
        public double Ufr { get; set; }

        [JsonPropertyName("convergence_maturity")]
        public int ConvergenceMaturity { get; set; }

        [JsonPropertyName("tol")]
        public double Tol { get; set; }
    }

    public class Response
    {
        public double alpha { get; set; }
        public List<double> rfr { get; set; }
    }
    public class RiskFreeRate
    {
        [DataType(DataType.Date)]
        public DateTime ValueDate { get; set; }
        public int Maturity { get; set; }
        public double Value { get; set; }
    }
}