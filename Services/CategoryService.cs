using FinanceTracker.Data;
using FinanceTracker.DTO.Categories;
using FinanceTracker.Models;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context; //?? throw new ArgumentNullException; надо ли это
    }
    
    
    public async Task<List<CategoryResponseDto>> GetAllAsync(CancellationToken ctt)
    {
        return await _context.Categories.Select(a => new CategoryResponseDto { Id = a.Id, Name = a.Name }).ToListAsync(ctt);
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id, CancellationToken ctt)
    {
        return await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponseDto { Id = c.Id, Name = c.Name }).FirstOrDefaultAsync(ctt);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto createCategoryDto, CancellationToken ctt)
    {
        var isCategoryTaken = await _context.Categories.AnyAsync(c => c.Name.ToLower() == createCategoryDto.Name.ToLower(), ctt);
        if (isCategoryTaken)
        {
            throw new InvalidOperationException("Категория с таким именем уже существует");
        }

        var category = new Category
        {
            Name = createCategoryDto.Name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(ctt);

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto, // тут использован классический способ обновления без ExecuteUpdateAsync как в AccountService; 
        CancellationToken ctt)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id, ctt);

        if (category is null)
            return null;

        category.Name = updateCategoryDto.Name;
        await _context.SaveChangesAsync(ctt);

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };

    }


    public async Task<bool> DeleteAsync(int id, CancellationToken ctt)
    {
        var rowsAffected = await _context.Categories.Where(c => c.Id == id).ExecuteDeleteAsync(ctt);

        return rowsAffected > 0;
    }
}