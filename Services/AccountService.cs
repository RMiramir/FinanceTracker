using FinanceTracker.Data;
using FinanceTracker.DTO;
using FinanceTracker.DTO.Accounts;
using FinanceTracker.Models;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
        
    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccountResponseDto>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Accounts.Select(a => new AccountResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Currency = a.Currency
            
        }).ToListAsync(ct);
    }

    public async Task<AccountResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Accounts.Where(a => a.Id == id).Select(a => new AccountResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Currency = a.Currency
        }).FirstOrDefaultAsync(ct);
    }

    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto createAccountDto, CancellationToken ct)
    {
        bool isNameTaken = await _context.Accounts
            .AnyAsync(a => a.Name.ToLower() == createAccountDto.Name.ToLower(), ct);

        if (isNameTaken)
        {
            throw new InvalidOperationException(
                $"Кошелёк с именем '{createAccountDto.Name}' уже существует");
        }
        var acc = new Account
        {
            Name = createAccountDto.Name,
            Currency = createAccountDto.Currency
        };

        _context.Accounts.Add(acc);

        await _context.SaveChangesAsync(ct);

        return MapToDto(acc);

    }

    public async Task<AccountResponseDto?> UpdateAsync(int id, UpdateAccountDto updateAccountDto, CancellationToken ct)
    {
        var rowsAffected = await _context.Accounts
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(t => t
                .SetProperty(a => a.Name, updateAccountDto.Name)
                .SetProperty(a => a.Currency, updateAccountDto.Currency), ct);

        if (rowsAffected == 0)
        {
            return null;
        }

        return new AccountResponseDto
        {
            Id = id,
            Name = updateAccountDto.Name,
            Currency = updateAccountDto.Currency
        };

    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var rowsAffected = await _context.Accounts.Where(a => a.Id == id).ExecuteDeleteAsync(ct);

        return rowsAffected > 0;
    }

    private static AccountResponseDto MapToDto(Account account)
    {
        return new AccountResponseDto
        {
            Id = account.Id,
            Name = account.Name,
            Currency = account.Currency
        };
    }
    
    
}