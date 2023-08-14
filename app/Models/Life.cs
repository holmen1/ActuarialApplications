using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActuarialApplications.Models;

public class ResponseCashFlow
{
    [JsonPropertyName("month")]
    public int Month { get; set; }
    [JsonPropertyName("benefit")]
    public double Benefit { get; set; }
}

public class CashFlow : ResponseCashFlow
{
    public DateTime ValueDate { get; set; }
    public int ContractNo { get; set; }
}

public class LifeIndexModel
{
    public SelectList ContractNoList { get; set; }
    public int SelectedContractNo { get; set; }
    public Contract Contract { get; set; }
    public double Age { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public double TechnicalProvision { get; set; }

    public List<CashFlow>? CashFlows { get; set; }
    public List<CashFlow>? DiscountedCashFlows { get; set; }
}