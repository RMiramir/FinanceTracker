using System.ComponentModel.DataAnnotations;
using FinanceTracker.Models;

namespace FinanceTracker.DTO.Transaction;

public class CreateTransactionDto
{
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; } 
    
    [Required]
    public TransactionType Type { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public List<int> TagIds { get; set; } = [];

}