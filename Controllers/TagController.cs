using FinanceTracker.DTO.Tag;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TagResponseDto>>> GetAll(CancellationToken ct)
    {

        return Ok(await _tagService.GetAllAsync(ct));
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TagResponseDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        return Ok(await _tagService.GetByIdAsync(id, ct));
    }

    [HttpPost]
    public async Task<ActionResult<TagResponseDto>> Create([FromBody] CreateTagDto createTagDto, CancellationToken ct)
    {
        var created = await _tagService.CreateAsync(createTagDto, ct);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TagResponseDto>> Update([FromRoute] int id, [FromBody] UpdateTagDto updateTagDto, CancellationToken ct)
    {
        return Ok(await _tagService.UpdateAsync(id, updateTagDto, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _tagService.DeleteAsync(id, ct);
        
        return NoContent();
    }
}