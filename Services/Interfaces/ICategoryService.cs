using FinanceTracker.DTO.Categories;

namespace FinanceTracker.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllAsync(CancellationToken ctt);
    Task<CategoryResponseDto> GetByIdAsync(int id, CancellationToken ctt);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto createCategoryDto, CancellationToken ctt);
    Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto, CancellationToken ctt);
    Task DeleteAsync(int id, CancellationToken ctt);

}