using FinanceTracker.DTO.Accounts;
using FluentResults;

namespace FinanceTracker.Services.Interfaces;

public interface IAccountService
{
    public Task<List<AccountResponseDto>> GetAllAsync(CancellationToken ct);
    
    public Task<AccountResponseDto> GetByIdAsync(int id, CancellationToken ct);
    
    public Task<AccountResponseDto> CreateAsync(CreateAccountDto createAccountDto, CancellationToken ct);
    public Task<AccountResponseDto> UpdateAsync(int id, UpdateAccountDto updateAccountDto, CancellationToken ct);
    
    public Task DeleteAsync(int id, CancellationToken ct);
}