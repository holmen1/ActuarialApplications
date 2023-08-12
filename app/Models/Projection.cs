using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActuarialApplications.Models;

public class RiskFreeRate
{
    [Key] public int ProjectionId { get; set; }
    [DataType(DataType.Date)] public DateTime ValueDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public string RequestParameters { get; set; }
    public string? VerifiedBy { get; set; }
}

public class RiskFreeRateData
{
    [Key] public int ProjectionId { get; set; }
    public int Month { get; set; }
    public double Maturity { get; set; }
    public double SpotValue { get; set; }
    public double Price { get; set; }
}

public class ProjectionIndexModel
{
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public SelectList ValueDates { get; set; }

    public DateTime SelectedDate { get; set; }
    public List<Swap> SwapRates { get; set; }
    public RiskFreeRate Param { get; set; }
    public List<RiskFreeRateData> Rfr { get; set; }
}