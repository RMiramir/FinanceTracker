namespace FinanceTracker.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; } 
    public TransactionType Type { get; set; }
    
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<TransactionTag> TransactionTags { get; set; } = new();
        
}