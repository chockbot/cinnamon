namespace Cinnamon.Web.Models.Entities;

public class Payouts
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public bool Status { get; set; }

    public decimal Amount { get; set; } 
}
