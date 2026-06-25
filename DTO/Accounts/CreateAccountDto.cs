using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DTO.Accounts;

public class CreateAccountDto
{
    [Required(ErrorMessage = "Имя кошелька не может быть пустым")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Укажите валюту")]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Валюта должна состоять из 3 заглавных букв (например: TJS, USD, RUB)")]
    public string Currency { get; set; } = null!;
}