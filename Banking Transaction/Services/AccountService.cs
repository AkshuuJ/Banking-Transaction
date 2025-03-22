using Microsoft.EntityFrameworkCore;

public interface IAccountService
{
    Task<Account> GetAccountByUserIdAsync(int userId);
    Task<Account> DepositAsync(int userId, decimal amount);
    Task<Account> WithdrawAsync(int userId, decimal amount);
    Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
}


public class AccountService : IAccountService
{
    private readonly BankingContext _context;

    public AccountService(BankingContext context)
    {
        _context = context;
    }

    public async Task<Account> GetAccountByUserIdAsync(int userId)
    {
        return await _context.Accounts
                              .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Account> DepositAsync(int userId, decimal amount)
    {
        var account = await GetAccountByUserIdAsync(userId);
        account.Balance += amount;
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<Account> WithdrawAsync(int userId, decimal amount)
    {
        var account = await GetAccountByUserIdAsync(userId);
        if (account.Balance >= amount)
        {
            account.Balance -= amount;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }
        else
        {
            throw new Exception("Insufficient funds");
        }
    }

    public async Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount)
    {
        var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
        var toAccount = await _context.Accounts.FindAsync(toAccountId);

        if (fromAccount.Balance >= amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            _context.Accounts.Update(fromAccount);
            _context.Accounts.Update(toAccount);

            var transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Date = DateTime.Now,
                Type = "Transfer"
            };
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();
            return transaction;
        }
        else
        {
            throw new Exception("Insufficient funds");
        }
    }
}
