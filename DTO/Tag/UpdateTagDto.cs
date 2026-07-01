using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DTO.Tag;

public class UpdateTagDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
}