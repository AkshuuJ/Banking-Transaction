public class Transaction
{
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } 
}
