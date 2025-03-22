using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAccount(int userId)
    {
        var account = await _accountService.GetAccountByUserIdAsync(userId);
        return account != null ? Ok(account) : NotFound();
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(int userId, [FromBody] decimal amount)
    {
        var account = await _accountService.DepositAsync(userId, amount);
        return Ok(account);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(int userId, [FromBody] decimal amount)
    {
        try
        {
            var account = await _accountService.WithdrawAsync(userId, amount);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(int fromAccountId, int toAccountId, [FromBody] decimal amount)
    {
        try
        {
            var transaction = await _accountService.TransferAsync(fromAccountId, toAccountId, amount);
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
