using Microsoft.EntityFrameworkCore;
using System.Text;
public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
    Task<string> ExportTransactionsToCsvAsync(int accountId);
}

public class TransactionService : ITransactionService
{
    private readonly BankingContext _context;

    public TransactionService(BankingContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
    {
        return await _context.Transactions
                             .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                             .ToListAsync();
    }

    public async Task<string> ExportTransactionsToCsvAsync(int accountId)
    {
        var transactions = await GetTransactionsByAccountIdAsync(accountId);

        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("TransactionId,FromAccountId,ToAccountId,Amount,Date,Type");

        foreach (var transaction in transactions)
        {
            csvBuilder.AppendLine($"{transaction.Id},{transaction.FromAccountId},{transaction.ToAccountId},{transaction.Amount},{transaction.Date},{transaction.Type}");
        }

        return csvBuilder.ToString();
    }
}
