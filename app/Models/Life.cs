namespace ActuarialApplications.Models;


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
}
