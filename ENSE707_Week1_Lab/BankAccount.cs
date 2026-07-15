namespace ENSE707_Week1_Lab;

public class BankAccount
{
    public string AccountHolder { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountHolder, decimal openingBalance)
    {
        AccountHolder = accountHolder;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance = Balance + amount;
    }

    public bool Withdraw(decimal amount)
    {
        Balance = Balance - amount;
        return true;
    }

    public decimal CalculateTransactionFee(decimal amount)
    {
        return amount * 0.02m;
    }
}