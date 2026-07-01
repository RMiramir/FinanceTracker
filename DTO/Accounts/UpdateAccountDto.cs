
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DTO.Accounts;

public class UpdateAccountDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;
    
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Валюта должна состоять из 3 заглавных букв (например: TJS, USD, RUB)")]
    public string Currency { get; set; } = null!;
}