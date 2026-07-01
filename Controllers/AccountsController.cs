using FinanceTracker.Data;
using FinanceTracker.DTO;
using FinanceTracker.DTO.Accounts;
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
        return Ok(await _accountService.GetAllAsync(ct));
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountResponseDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        return Ok(await _accountService.GetByIdAsync(id, ct)); 
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponseDto>> Create([FromBody] CreateAccountDto accountDto, CancellationToken ct)
    {
        var createdAccount = await _accountService.CreateAsync(accountDto, ct);
        
        return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AccountResponseDto>> Update([FromRoute] int id, [FromBody] UpdateAccountDto updateAccountDto, CancellationToken ct)
    {
        return Ok(await _accountService.UpdateAsync(id, updateAccountDto, ct));
    }
    
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        await _accountService.DeleteAsync(id, ct);
        return NoContent();
    }
    
    
}