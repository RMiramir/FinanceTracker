using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.DTO.Categories;

public class CreateCategoryDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";
}