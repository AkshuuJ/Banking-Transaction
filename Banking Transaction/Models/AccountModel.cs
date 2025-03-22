public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }  // Foreign Key to User
    public User User { get; set; }
}
