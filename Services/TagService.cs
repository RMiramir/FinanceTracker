using FinanceTracker.Data;
using FinanceTracker.DTO.Tag;
using FinanceTracker.Exceptions;
using FinanceTracker.Models;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class TagService : ITagService
{
    private readonly AppDbContext _context;

    public TagService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TagResponseDto>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Tags.Select(t => new TagResponseDto
        {
            Id = t.Id,
            Name = t.Name
        }).ToListAsync(ct);
    }

    public async Task<TagResponseDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var tag = await _context.Tags.Where(t => t.Id == id).Select(t => new TagResponseDto
        {
            Id = t.Id,
            Name = t.Name
        }).FirstOrDefaultAsync(ct);

        if (tag is null)
            throw new NotFoundException($"Ярлык с таким ID {id} не найден");

        return tag;
    }

    public async Task<TagResponseDto> CreateAsync(CreateTagDto dto, CancellationToken ct)
    {
        var isTagTaken = await _context.Tags.AnyAsync(t => t.Name.ToLower() == dto.Name.ToLower(), ct);

        if (isTagTaken)
        {
            throw new ConflictException($"Ярлык с таким именем {dto.Name} уже существует");
        }

        var newTag = new Tag
        {
            Name = dto.Name
        };

        _context.Tags.Add(newTag);
        await _context.SaveChangesAsync(ct);

        return new TagResponseDto
        {
            Id = newTag.Id,
            Name = newTag.Name
        };


    }

    public async Task<TagResponseDto> UpdateAsync(int id, UpdateTagDto dto, CancellationToken ct)
    {
        var updateTad = _context.Tags.FirstOrDefault(t => t.Id == id);

        if (updateTad is null)
            throw new NotFoundException($"Ярлык с таким ID {id} не найден");

        updateTad.Name = dto.Name;
        await _context.SaveChangesAsync(ct);

        return new TagResponseDto
        {
            Id = updateTad.Id,
            Name = updateTad.Name
        };

        /*var rowsAffected = await _context.Tags.Where(t => t.Id == id)
            .ExecuteUpdateAsync(t => t
                .SetProperty(tt => tt.Name, dto.Name), ct);

        if (rowsAffected == 0)
            return null;

        return new TagResponseDto
        {
            Id = id,
            Name = dto.Name
        };*/
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var isDeleted = await _context.Tags.Where(t => t.Id == id).ExecuteDeleteAsync(ct);

        if(isDeleted == 0)
            throw new NotFoundException($"Ярлык с таким ID {id} не найден");
    }
    
}