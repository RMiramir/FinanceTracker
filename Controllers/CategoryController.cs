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
        var category = await _categoryService.GetByIdAsync(id, ctt);

        return category is not null
            ? Ok(category)
            : NotFound(new { message = $"Категория с таким ID {id} не существует" });
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
        var updateCategory = await _categoryService.UpdateAsync(id, updateCategoryDto, ctt);

        if (updateCategory is null)
        {
            return NotFound(new { message = $"Категория с таким ID {id} не найдена" });
        }

        return Ok(updateCategory);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ctt)
    {
        return await _categoryService.DeleteAsync(id, ctt)
            ? NoContent()
            : NotFound(new { message = $"Категория с таким ID {id} не найдена" });
    }
    
    
    
}