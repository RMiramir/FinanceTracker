using FinanceTracker.DTO.Categories;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace FinanceTracker.Controllers;


[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDto>>> GetAll(CancellationToken ctt)
    {
        return Ok(await _categoryService.GetAllAsync(ctt));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById([FromRoute] int id, CancellationToken ctt)
    {
        return Ok(await _categoryService.GetByIdAsync(id, ctt));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create([FromBody]CreateCategoryDto createCategoryDto,
        CancellationToken ctt)
    {
        var createdCategory = await _categoryService.CreateAsync(createCategoryDto, ctt);

        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> Update([FromRoute] int id, [FromBody] UpdateCategoryDto updateCategoryDto,
        CancellationToken ctt)
    {
        return Ok(await _categoryService.UpdateAsync(id, updateCategoryDto, ctt));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ctt)
    {
        await _categoryService.DeleteAsync(id, ctt);
        
        return NoContent();
    }
    
    
    
}