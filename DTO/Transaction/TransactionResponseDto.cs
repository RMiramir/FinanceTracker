using System.Runtime.InteropServices.JavaScript;
using FinanceTracker.DTO.Tag;
using FinanceTracker.Models;

namespace FinanceTracker.DTO.Transaction;

public class TransactionResponseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    
    public int AccountId { get; set; }
    public string AccountName { get; set; } = "";
    
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";

    public List<TagResponseDto> Tags { get; set; } = [];
}