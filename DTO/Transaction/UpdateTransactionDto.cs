using FinanceTracker.Models;

namespace FinanceTracker.DTO.Transaction;


public class UpdateTransactionDto
{
    public decimal Amount { get; set; }

    public string Description { get; set; } = "";
    
    public TransactionType Type { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public List<int> TagIds { get; set; } = [];
}