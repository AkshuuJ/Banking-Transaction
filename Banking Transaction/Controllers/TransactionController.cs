using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetTransactions(int accountId)
    {
        var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
        return Ok(transactions);
    }

    [HttpGet("export/{accountId}")]
    public async Task<IActionResult> ExportTransactionsToCsv(int accountId)
    {
        var csvData = await _transactionService.ExportTransactionsToCsvAsync(accountId);

        // Return CSV content as a file
        var fileName = $"transactions_{accountId}.csv";
        return File(Encoding.UTF8.GetBytes(csvData), "text/csv", fileName);
    }
}
