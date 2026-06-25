namespace FinanceTracker.Models;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public required string Currency { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}