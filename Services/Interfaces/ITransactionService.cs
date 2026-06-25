using FinanceTracker.DTO.Transaction;
using FinanceTracker.Models;

namespace FinanceTracker.Services.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionResponseDto>> GetAllAsync(CancellationToken ct);

    Task<TransactionResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, CancellationToken ct);

    Task<TransactionResponseDto?> UpdateAsync(int id, UpdateTransactionDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByAccountAsync(int id, CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByCategoryAsync(int id, CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByIncomeAsync(CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByExpenseAsync(CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct);

    Task<List<TransactionResponseDto>> GetByTagAsync(int tagId, CancellationToken ct);

    Task<Transaction?> GetEntityByIdAsync(int id, CancellationToken ct);



}