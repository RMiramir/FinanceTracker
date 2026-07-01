using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DTO.Tag;

public class CreateTagDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";
}