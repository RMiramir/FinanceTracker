namespace FinanceTracker.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public List<TransactionTag> TransactionTags { get; set; } = new();

}