//namespace ENSE707_Week1_Lab;

//public class BankAccount
//{
//    public string AccountHolder { get; set; }
//    public decimal Balance { get; private set; }

//    public BankAccount(string accountHolder, decimal openingBalance)
//    {
//        AccountHolder = accountHolder;
//        Balance = openingBalance;
//    }

//    public void Deposit(decimal amount)
//    {
//        Balance = Balance + amount;
//    }

//    public bool Withdraw(decimal amount)
//    {
//        Balance = Balance - amount;
//        return true;
//    }

//    public decimal CalculateTransactionFee(decimal amount)
//    {
//        return amount * 0.02m;
//    }
//}

namespace ENSE707_Week1_Lab;

public class BankAccount
{
    public string AccountHolder { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountHolder, decimal openingBalance)
    {
        if (string.IsNullOrWhiteSpace(accountHolder))
            throw new ArgumentException("Account holder name is required.");
        if (openingBalance < 0)
            throw new ArgumentException("Opening balance cannot be negative.");

        AccountHolder = accountHolder;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");
        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");
        if (amount > Balance)
            return false;

        Balance -= amount;
        return true;
    }

    public decimal CalculateTransactionFee(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be positive.");
        return Math.Round(amount * 0.02m, 2);
    }
}