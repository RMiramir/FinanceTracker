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
        var response = await _transactionService.GetAllAsync(ct);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionResponseDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        var transaction = await _transactionService.GetByIdAsync(id, ct);

        return Ok(transaction);

    }
    
    
    [HttpPost("create")]
    public async Task<ActionResult<TransactionResponseDto>> Create([FromBody] CreateTransactionDto createTransactionDto,
        CancellationToken ct)
    {
        var response = await _transactionService.CreateAsync(createTransactionDto, ct);

        return CreatedAtAction(nameof(GetById), new {id = response.Id}, response);
    }

    [HttpPut("update")]
    public async Task<ActionResult<TransactionResponseDto>> Update([FromRoute] int id,
        [FromBody] UpdateTransactionDto updateTransactionDto, CancellationToken ct)
    {
        var response = await _transactionService.UpdateAsync(id, updateTransactionDto, ct);
        if (response is null)
        {
            return BadRequest(new { message = "Транзакция с таким именем не найден" });
        }
        
        return Ok(response);

    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        return await _transactionService.DeleteAsync(id, ct) ? NoContent(): NotFound(new { message = $"Tag с таким Id {id} не найден" });
    }

    [HttpGet("getByAccount")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByAccount([FromRoute] int id, CancellationToken ct)
    {
        var listTransactionsByAccount = await _transactionService.GetByAccountAsync(id, ct);

        return Ok(listTransactionsByAccount);

    }

    [HttpGet("getByCategory")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByCategory(int id, CancellationToken ct)
    {
        var listTransactionsGetByCategory = _transactionService.GetByCategoryAsync(id, ct);

        return Ok(listTransactionsGetByCategory);
    }

    [HttpGet("getByIncome")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByIncome(CancellationToken ct)
    {

        var listTransactionGetByIncome = _transactionService.GetByIncomeAsync(ct);

        return Ok(listTransactionGetByIncome);

    }
    
    [HttpGet("getByExpense")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByExpense(int id, CancellationToken ct)
    {

        var listTransactionGetByExpense = _transactionService.GetByExpenseAsync(ct);

        return Ok(listTransactionGetByExpense);

    }

    [HttpGet("getByDate")]
    public async Task<ActionResult<List<TransactionResponseDto>>> GetByDateRange(
        [FromQuery(Name = "from")] DateTime? from = null, // Теперь это не обязательный параметр
        [FromQuery(Name = "to")] DateTime? to = null,   // Если их не передадут, они будут равны null
        CancellationToken ct = default)
    {
        var dateFrom = from ?? DateTime.UtcNow.AddDays(-1);
        var dateTo = to ?? DateTime.UtcNow;

        return Ok(await _transactionService.GetByDateRangeAsync(dateFrom, dateTo, ct));
    }

    [HttpGet("getByTag/{id:int}")]
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