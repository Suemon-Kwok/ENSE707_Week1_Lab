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
    // Improvement: AccountHolder is now read-only after construction (no public setter).
    // Previously { get; set; } allowed external code to change the name at any time.
    public string AccountHolder { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string accountHolder, decimal openingBalance)
    {
        // Improvement: reject blank/empty account holder names.
        // Original code accepted any string, including "" or whitespace.
        if (string.IsNullOrWhiteSpace(accountHolder))
            throw new ArgumentException("Account holder name is required.");

        // Improvement: reject a negative opening balance.
        // Original code allowed accounts to be created already "in the hole."
        if (openingBalance < 0)
            throw new ArgumentException("Opening balance cannot be negative.");

        AccountHolder = accountHolder;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        // Improvement: reject zero/negative deposits.
        // Original code let a "deposit" of a negative number silently reduce the balance.
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        // Improvement: reject zero/negative withdrawal amounts.
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");

        // Improvement: prevent overdrafts. Original code always subtracted and
        // returned true, even when amount > Balance, letting Balance go negative.
        if (amount > Balance)
            return false;

        Balance -= amount;
        return true;
    }

    public decimal CalculateTransactionFee(decimal amount)
    {
        // Improvement: reject zero/negative transaction amounts.
        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be positive.");

        // Improvement: round to 2 decimal places for correct currency formatting.
        // Original code returned the raw multiplication with no rounding.
        return Math.Round(amount * 0.02m, 2);
    }
}

/*
Correctness:  improved.
The original code let Withdraw succeed even when it drove the balance negative, and CalculateTransactionFee never rounded (so results like 100 * 0.02m could carry extra decimal places in edge cases). The improved version fixes both: Withdraw now checks if (amount > Balance) return false; before ever touching Balance, and CalculateTransactionFee wraps the result in Math.Round(amount * 0.02m, 2). The class now behaves the way the requirements actually describe (no overdrafts, clean 2-decimal fees).

Reliability: improved.
Every public method that takes a decimal amount now guards against bad input before doing any work: Deposit, Withdraw, and CalculateTransactionFee all start with if (amount <= 0) throw new ArgumentException(...). The constructor does the same for openingBalance < 0 and a blank accountHolder. This means the object can never silently drift into an invalid state (negative balance, empty name) — previously any of these could happen without warning.

Maintainability: improved.
The validation logic is explicit and localized right at the top of each method, in plain, self-documenting if statements with descriptive exception messages (e.g. "Opening balance cannot be negative."). A future developer reading this code doesn't have to guess the rules — they're written directly into the guard clauses, so there's no separate spec to hunt down or reverse-engineer from behavior.

Testability: improved.
This is the biggest structural change. Before, invalid input just produced silently wrong output (e.g., a negative balance after an overdraft) — there was no clean way to assert "this failed correctly." Now every invalid-input path throws a specific typed exception (ArgumentException), which can be tested directly with Assert.ThrowsException<ArgumentException>(...) — exactly the pattern in your Activity 8 tests. Also, AccountHolder changed from { get; set; } to { get; } (read-only after construction), which removes a way external code could mutate state outside the class's control — a small testability/encapsulation win.

Security: modestly improved, and worth being honest about the limits.
Preventing overdrafts and rejecting negative amounts is a form of input validation, which is a basic security hygiene practice (garbage-in prevention). But this is a minor, narrow improvement — there's no authentication, authorization, encryption, or protection against concurrent-access issues here, so I wouldn't overstate this one in your write-up. A fair sentence: "Input validation reduces the risk of invalid data corrupting account state, but broader security concerns like access control are out of scope for this class."

User trust: improved.
From a bank-staff-user perspective, the account will now refuse to let a customer overdraw, refuse a "deposit" of a negative number, and refuse to create an account with a negative starting balance or no name — all with a clear explanation of why, via the exception message. This directly builds confidence the system won't silently produce a wrong balance, which was the core problem for a "small community bank" per your Problem Statement.
 */