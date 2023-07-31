using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ActuarialApplications.Models
{
    public class Contract
    {
        [JsonPropertyName("contractNo")]
        public int ContractNo { get; set; }
        [JsonPropertyName("valueDate")]
        public DateTime ValueDate { get; set; }
        [JsonPropertyName("birthDate")]
        public DateTime BirthDate { get; set; }
        [JsonPropertyName("sex")]
        public string Sex { get; set; } // M or F
        [JsonPropertyName("z")]
        public int VestingAge { get; set; }
        [JsonPropertyName("guarantee")]
        [Column("Guarantee")]
        public double GuaranteeBenefit { get; set; }
        [JsonPropertyName("payPeriod")]
        public int PayPeriod { get; set; }
        [JsonPropertyName("table")]
        public string Table { get; set; } // AP or APG
    }
}