using FinanceTracker.DTO.Tag;

namespace FinanceTracker.Services.Interfaces;

public interface ITagService
{
    Task<List<TagResponseDto>> GetAllAsync(CancellationToken ct);

    Task<TagResponseDto> GetByIdAsync(int id, CancellationToken ct);

    Task<TagResponseDto> CreateAsync(CreateTagDto dto, CancellationToken ct);

    Task<TagResponseDto> UpdateAsync(int id, UpdateTagDto dto, CancellationToken ct);

    Task DeleteAsync(int id, CancellationToken ct);
    
}
