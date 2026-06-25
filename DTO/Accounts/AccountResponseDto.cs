using FinanceTracker.Models;

namespace FinanceTracker.DTO.Accounts;

public class AccountResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
}