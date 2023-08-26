using System.ComponentModel.DataAnnotations;

namespace ActuarialApplications.Models;

public class Swap
{
    [DataType(DataType.Date)] public DateTime ValueDate { get; set; }
    public string Id { get; set; }
    public int Tenor { get; set; }
    public double? SettlementFreq { get; set; }
    public double Value { get; set; }
}