using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActuarialApplications.Models
{
    public class FastApiRequestParameters
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
        [JsonPropertyName("blaha")]
        public int Blaha { get; set; }
    }

    public class FastApiResponse
    {
        public double alpha { get; set; }
        public List<double> rfr { get; set; }
    }
    public class RiskFreeRate
    {
        [Key]
        public int ProjectionId { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValueDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public string RequestParameters { get; set; }
        public string? VerifiedBy { get; set; }
    }
    
    public class RiskFreeRateData
    {
        [Key]
        public int ProjectionId { get; set; }
        public int Maturity { get; set; }
        public double SpotValue { get; set; }
    }

    public class ProjectionIndexModel
    {
        public SelectList ValueDates{ get; set; }
        public DateTime SelectedDate { get; set; }
        public List<Swap> SwapRates { get; set; }
        public double Ufr { get; set; }
        public int ConvergenceMaturity { get; set; }
        public RiskFreeRate Param { get; set; }
        public List<RiskFreeRateData> Rfr { get; set; }
    }
}