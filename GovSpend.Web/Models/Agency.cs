namespace GovSpend.Web.Models;
public class Agency
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TotalSpending { get; set; }
    public int ContractCount { get; set; }
    public bool DemoMode {get; } 
}