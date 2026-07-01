using FinanceTracker.Data;
using FinanceTracker.DTO.Tag;
using FinanceTracker.DTO.Transaction;
using FinanceTracker.Exceptions;
using FinanceTracker.Models;
using FinanceTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FinanceTracker.Services;

public class TransactionService : ITransactionService
{

    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TransactionResponseDto>> GetAllAsync(CancellationToken ct)
    {
        var transactions = await _context.Transactions
            .Include(a => a.Account)
            .Include(c => c.Category)
            .Include(t => t.TransactionTags)
                .ThenInclude(tt => tt.Tag).ToListAsync(ct);
        
        
        var list = new List<TransactionResponseDto>();
        foreach (var t in transactions)
        {
            var response = new TransactionResponseDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description ?? "",
                Date = t.Date,
                Type = t.Type,
                AccountId = t.AccountId,
                AccountName = t.Account.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Tags = t.TransactionTags.Select(tt => new TagResponseDto
                {
                    Id = tt.Tag.Id,
                    Name = tt.Tag.Name
                }).ToList()
            };
            
            list.Add(response);

        }

        return list;

        /*var trans = await _context.Transactions.Select(t => new TransactionResponseDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Description = t.Description ?? "",
            Date = t.Date,
            Type = t.Type,
            AccountId = t.AccountId,
            AccountName = t.Account.Name,
            CategoryId = t.CategoryId,
            CategoryName = t.Category.Name,
            Tags = t.TransactionTags.Select(tt => new TagResponseDto
            {
                Id = tt.Tag.Id,
                Name = tt.Tag.Name
            }).ToList()
        }).ToListAsync(ct);*/


    }

    public async Task<TransactionResponseDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var transaction = await _context.Transactions.Where(t => t.Id == id)
            .Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description ?? "",
                Date = t.Date,
                Type = t.Type,
                AccountId = t.AccountId,
                AccountName = t.Account.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Tags = t.TransactionTags.Select(a =>new TagResponseDto
                {
                    Id = a.Tag.Id,
                    Name = a.Tag.Name
                }).ToList()
            }).FirstOrDefaultAsync(ct);

        if (transaction is null)
            throw new NotFoundException($"Транзакция с таким ID {id} не найдена");

        return transaction;

    }

    public async Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, CancellationToken ct)
    {
        var accountExists = await _context.Accounts.AnyAsync(t => t.Id == dto.AccountId, ct);
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId, ct);
        var existingTags = await _context.Tags.CountAsync(b => dto.TagIds.Contains(b.Id), ct);

        if (!accountExists || !categoryExists || existingTags != dto.TagIds.Distinct().Count())
        {
            throw new ConflictException($"Транзакция с таким AccountId, CategoryId или TagIds уже существует");
        }
        
        var newTransaction = new Transaction
        {
            Amount = dto.Amount,
            Description = dto.Description,
            Date = DateTime.UtcNow,
            Type = dto.Type,
            AccountId = dto.AccountId,
            CategoryId = dto.CategoryId,
            /*TransactionTags = dto.TagIds.Select(t=>new TransactionTag
            {
                TagId = t
            }).ToList()*/ // второй способ без повторного await _context.SaveChangesAsync(ct);
        };
        
        
        
       
        _context.Transactions.Add(newTransaction);

        await _context.SaveChangesAsync(ct);

        foreach (var tagId in dto.TagIds.Distinct())
        {
            var transactionTag = new TransactionTag
            {
                TransactionId = newTransaction.Id,
                TagId = tagId
            };
            _context.TransactionTags.Add(transactionTag);

        }

        await _context.SaveChangesAsync(ct);

        return new TransactionResponseDto
        {
            Id = newTransaction.Id,
            Amount = newTransaction.Amount,
            Description = newTransaction.Description ?? "",
            Date = newTransaction.Date,
            Type = newTransaction.Type,
            AccountId = newTransaction.AccountId,
            AccountName = "",
            CategoryId = newTransaction.CategoryId,
            CategoryName = "",
            Tags = dto.TagIds.Select(id => new TagResponseDto
            {
                Id = id,
                Name = ""
            }).ToList()
        };


    }

    public async Task<TransactionResponseDto> UpdateAsync(int id, UpdateTransactionDto dto, CancellationToken ct)
    {
        var updateTransaction = await _context.Transactions
            .Include(t => t.TransactionTags)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (updateTransaction is null)
            throw new NotFoundException($"Транзакция с таким ID {id} не найдена");

        var accountExists =await _context.Accounts.AnyAsync(t => t.Id == dto.AccountId, ct);
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId, ct);
        var uniqueTags = dto.TagIds.Distinct().ToList();
        var existingTagsCount = await _context.Tags.CountAsync(t => uniqueTags.Contains(t.Id), ct);
        
        if (!accountExists || !categoryExists || existingTagsCount != uniqueTags.Count())
            throw new ConflictException($"Транзакция с таким AccountId, CategoryId или TagIds уже существует");
        
        
        updateTransaction.Amount = dto.Amount;
        updateTransaction.Description = dto.Description;
        updateTransaction.Type = dto.Type;
        updateTransaction.AccountId = dto.AccountId;
        updateTransaction.CategoryId = dto.CategoryId;

        foreach (var tag in uniqueTags)
        {
            updateTransaction.TransactionTags.Add(new TransactionTag
            {
                TagId = tag
            });
        }

        await _context.SaveChangesAsync(ct);

        return new TransactionResponseDto
        {
            Id = updateTransaction.Id,
            Amount = dto.Amount,
            Description = dto.Description ?? "",
            Type = dto.Type,
            AccountId = dto.AccountId,
            AccountName = "", // updateTransaction.Account.Name - это неправильно ведь дынные могли поменяться
            CategoryId = dto.CategoryId,
            CategoryName = "",
            Tags = uniqueTags.Select(t => new TagResponseDto
            {
                Id = t,
                Name = ""
            }).ToList()
        };

    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var rowsAffected = await _context.Transactions.Where(t => t.Id == id).ExecuteDeleteAsync(ct);
        
        if (rowsAffected == 0)
            throw new NotFoundException($"Транзакция с таким ID {id} не найдена");
    }

    public async Task<List<TransactionResponseDto>> GetByAccountAsync(int id, CancellationToken ct)
    {
        return await _context.Transactions.Where(t => t.AccountId == id).Select(tt => new TransactionResponseDto
        {
            Id = tt.Id,
            Amount = tt.Amount,
            Description = tt.Description ?? "",
            Date = tt.Date,
            Type = tt.Type,
            AccountId = tt.AccountId,
            AccountName = tt.Account.Name,
            CategoryId = tt.CategoryId,
            CategoryName = tt.Category.Name,
            Tags = tt.TransactionTags.Select(a => new TagResponseDto
            {
                Id = a.Tag.Id,
                Name = a.Tag.Name
            }).ToList()
        }).ToListAsync(ct);
    }

    public async Task<List<TransactionResponseDto>> GetByCategoryAsync(int id, CancellationToken ct)
    {
        var transactionsByCategory = await _context.Transactions.Where(c => c.CategoryId == id).Select(tt => new TransactionResponseDto
        {
            Id = tt.Id,
            Amount = tt.Amount,
            Description = tt.Description ?? "",
            Date = tt.Date,
            Type = tt.Type,
            AccountId = tt.AccountId,
            AccountName = tt.Account.Name,
            CategoryId = tt.CategoryId,
            CategoryName = tt.Category.Name,
            Tags = tt.TransactionTags.Select(a => new TagResponseDto
            {
                Id = a.Tag.Id,
                Name = a.Tag.Name
            }).ToList()
        }).ToListAsync(ct);

        return transactionsByCategory;
    }

    public Task<List<TransactionResponseDto>> GetByIncomeAsync(CancellationToken ct)
    {
        return _context.Transactions.Where(i => i.Type == TransactionType.Income)
            .Select(tt => new TransactionResponseDto
            {
                Id = tt.Id,
                Amount = tt.Amount,
                Description = tt.Description ?? "",
                Date = tt.Date,
                Type = tt.Type,
                AccountId = tt.AccountId,
                AccountName = tt.Account.Name,
                CategoryId = tt.CategoryId,
                CategoryName = tt.Category.Name,
                Tags = tt.TransactionTags.Select(a => new TagResponseDto
                {
                    Id = a.Tag.Id,
                    Name = a.Tag.Name
                }).ToList()
            }).ToListAsync(ct);
    }

    public Task<List<TransactionResponseDto>> GetByExpenseAsync(CancellationToken ct)
    {
        
        return _context.Transactions.Where(i => i.Type == TransactionType.Expense)
            .Select(tt => new TransactionResponseDto
            {
                Id = tt.Id,
                Amount = tt.Amount,
                Description = tt.Description ?? "",
                Date = tt.Date,
                Type = tt.Type,
                AccountId = tt.AccountId,
                AccountName = tt.Account.Name,
                CategoryId = tt.CategoryId,
                CategoryName = tt.Category.Name,
                Tags = tt.TransactionTags.Select(a => new TagResponseDto
                {
                    Id = a.Tag.Id,
                    Name = a.Tag.Name
                }).ToList()
            }).ToListAsync(ct);
    }
    
    public Task<List<TransactionResponseDto>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return _context.Transactions
            .Where(d => d.Date >= from.Date && d.Date <= to.Date.AddDays(1))
            .Select(tt => new TransactionResponseDto
            {
                Id = tt.Id,
                Amount = tt.Amount,
                Description = tt.Description ?? "",
                Date = tt.Date,
                Type = tt.Type,
                AccountId = tt.AccountId,
                AccountName = tt.Account.Name,
                CategoryId = tt.CategoryId,
                CategoryName = tt.Category.Name,
                Tags = tt.TransactionTags.Select(a => new TagResponseDto
                {
                    Id = a.Tag.Id,
                    Name = a.Tag.Name
                }).ToList()
            }).ToListAsync(ct);
    }

    public async Task<List<TransactionResponseDto>> GetByTagAsync(int tagId, CancellationToken ct)
    {
        return await _context.Transactions
            .Where(t => t.TransactionTags.Any(i => i.TagId == tagId))
            .Select(tt => new TransactionResponseDto
            {
                Id = tt.Id,
                Amount = tt.Amount,
                Description = tt.Description ?? "",
                Date = tt.Date,
                Type = tt.Type,
                AccountId = tt.AccountId,
                AccountName = tt.Account.Name,
                CategoryId = tt.CategoryId,
                CategoryName = tt.Category.Name,
                Tags = tt.TransactionTags.Select(a => new TagResponseDto
                {
                    Id = a.Tag.Id,
                    Name = a.Tag.Name
                }).ToList()
            }).ToListAsync(ct);
    }

    public async Task<Transaction> GetEntityByIdAsync(int id, CancellationToken ct)
    {
        var transaction = await _context.Transactions
            .Include(a => a.Account)
            .Include(c => c.Category)
            .Include(t => t.TransactionTags)
            .ThenInclude(tt => tt.Tag).FirstOrDefaultAsync(r => r.Id == id, ct);

        if (transaction is null)
        {
            throw new NotFoundException($"Транзакция с таким ID {id} не найдена");
        }
        
        return transaction;
    }
}