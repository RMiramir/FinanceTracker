using FinanceTracker.Data;
using FinanceTracker.DTO;
using FinanceTracker.Models;
using FinanceTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountResponseDto>>> GetAll(CancellationToken ct)
    {
        var accounts = await _accountService.GetAllAsync(ct);

        return Ok(accounts);

    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountResponseDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        var accById = await _accountService.GetByIdAsync(id, ct);
        if (accById is null)
        {
            return NotFound(new { message = $"Кошелёк с таким ID {id} не найден" });
        }

        return Ok(accById); 
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponseDto>> Create([FromBody] CreateAccountDto accountDto, CancellationToken ct)
    {
        try
        {
            var createdAccount = await _accountService.CreateAsync(accountDto, ct);
            
            return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AccountResponseDto>> Update([FromRoute] int id, [FromBody] UpdateAccountDto updateAccountDto, CancellationToken ct)
    {
        var updateAcc = await _accountService.UpdateAsync(id, updateAccountDto, ct);
        if (updateAcc is null)
        {
            return NotFound(new { message = $"Кошелёк с таким ID {id} не найден" });
        }

        return Ok(updateAcc);

    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        return await _accountService.DeleteAsync(id, ct)
            ? NoContent()
            : NotFound(new { message = $"Кошелёк с таким ID {id} не найден" });
    }
    
    
}