using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActuarialApplications.Models;

public class CashFlow
{
    public DateTime ValueDate { get; set; }
    public int ContractNo { get; set; }
    public int Month { get; set; }
    public double Benefit { get; set; }
}

public class LifeIndexModel
{
    public SelectList contractNoList { get; set; }
    public int SelectedContractNo { get; set; }
    public Contract contract { get; set; }
    public List<CashFlow> cashFlows { get; set; }
}