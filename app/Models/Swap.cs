using System.ComponentModel.DataAnnotations;

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
}