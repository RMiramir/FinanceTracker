using System.Transactions;
using FinanceTracker.DTO.Transaction;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetAll(CancellationToken ct)
    {
        return Ok(await _transactionService.GetAllAsync(ct));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionResponseDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        return Ok(await _transactionService.GetByIdAsync(id, ct));
    }
    
    
    [HttpPost("create")]
    public async Task<ActionResult<TransactionResponseDto>> Create([FromBody] CreateTransactionDto createTransactionDto,
        CancellationToken ct)
    {
        var response = await _transactionService.CreateAsync(createTransactionDto, ct);

        return CreatedAtAction(nameof(GetById), new {id = response.Id}, response);
    }

    [HttpPut("update/{id:int}")]
    public async Task<ActionResult<TransactionResponseDto>> Update([FromRoute] int id,
        [FromBody] UpdateTransactionDto updateTransactionDto, CancellationToken ct)
    {
        return Ok(await _transactionService.UpdateAsync(id, updateTransactionDto, ct));
    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        await _transactionService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpGet("by-account/{id:int}")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByAccount([FromRoute] int id, CancellationToken ct)
    {
       return Ok(await _transactionService.GetByAccountAsync(id, ct));

    }

    [HttpGet("by-category")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByCategory(int id, CancellationToken ct)
    {
        var listTransactionsGetByCategory = await _transactionService.GetByCategoryAsync(id, ct);

        return Ok(listTransactionsGetByCategory);
    }

    [HttpGet("by-income")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByIncome(CancellationToken ct)
    {

        var listTransactionGetByIncome = await _transactionService.GetByIncomeAsync(ct);

        return Ok(listTransactionGetByIncome);

    }
    
    [HttpGet("by-expense")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByExpense(CancellationToken ct)
    {

        var listTransactionGetByExpense = await _transactionService.GetByExpenseAsync(ct);

        return Ok(listTransactionGetByExpense);

    }

    [HttpGet("by-date")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByDateRange(
        [FromQuery(Name = "from")] DateTime? from = null, // Теперь это не обязательный параметр
        [FromQuery(Name = "to")] DateTime? to = null,   // Если их не передадут, они будут равны null
        CancellationToken ct = default)
    {
        var dateFrom = from ?? DateTime.UtcNow.AddDays(-1);
        var dateTo = to ?? DateTime.UtcNow;

        return Ok(await _transactionService.GetByDateRangeAsync(dateFrom, dateTo, ct));
    }

    [HttpGet("by-tag/{id:int}")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByTag([FromRoute] int id, CancellationToken ct)
    {
        var listTransactionGetByTag = await _transactionService.GetByTagAsync(id, ct);

        return Ok(listTransactionGetByTag);
    }

    [HttpGet("entity/{entityId:int}")]
    public async Task<ActionResult<Transaction>> GetEntity([FromRoute(Name = "entityId")] int id, CancellationToken ct)
    {
        var entity = await _transactionService.GetEntityByIdAsync(id, ct);
        if (entity is null)
            return NotFound(new { message = $"Транзакция с таким ID {id} не существует" });

        return Ok(entity);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}