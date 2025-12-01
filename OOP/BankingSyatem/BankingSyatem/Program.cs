using System;
using System.Collections.Generic;
using System.Linq;

// ============================================
// BANKING SYSTEM - COMPLETE OOP TUTORIAL
// ============================================

// ============================================
// ABSTRACTION: Abstract base class for accounts
// ============================================
public abstract class Account
{
    // Auto-implemented properties (ENCAPSULATION)
    public string AccountNumber { get; set; }
    public Customer AccountHolder { get; set; }
    public decimal Balance { get; protected set; }
    public DateTime CreationDate { get; private set; }
    public bool IsActive { get; set; }
    protected List<Transaction> Transactions { get; set; }

    // Constructor
    protected Account(string accountNumber, Customer accountHolder, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        AccountHolder = accountHolder;
        Balance = initialBalance;
        CreationDate = DateTime.Now;
        IsActive = true;
        Transactions = new List<Transaction>();

        // Record initial deposit
        if (initialBalance > 0)
        {
            Transactions.Add(new Transaction(TransactionType.Deposit, initialBalance,
                "Initial deposit", null, this));
        }
    }

    // Abstract methods - must be implemented by derived classes
    public abstract decimal CalculateInterest();
    public abstract string GetAccountType();

    // Virtual methods - can be overridden
    public virtual bool Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("❌ Deposit amount must be positive!");
            return false;
        }

        Balance += amount;
        Transactions.Add(new Transaction(TransactionType.Deposit, amount,
            "Deposit", null, this));
        Console.WriteLine($"✓ Deposited ${amount:N2}. New balance: ${Balance:N2}");
        return true;
    }

    public virtual bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("❌ Withdrawal amount must be positive!");
            return false;
        }

        if (amount > Balance)
        {
            Console.WriteLine("❌ Insufficient funds!");
            return false;
        }

        Balance -= amount;
        Transactions.Add(new Transaction(TransactionType.Withdrawal, amount,
            "Withdrawal", this, null));
        Console.WriteLine($"✓ Withdrew ${amount:N2}. New balance: ${Balance:N2}");
        return true;
    }

    public List<Transaction> GetTransactionHistory()
    {
        return new List<Transaction>(Transactions);
    }

    public virtual void DisplayAccountInfo()
    {
        Console.WriteLine($"Account: {AccountNumber} ({GetAccountType()})");
        Console.WriteLine($"Holder: {AccountHolder?.FullName}");
        Console.WriteLine($"Balance: ${Balance:N2}");
        Console.WriteLine($"Status: {(IsActive ? "Active" : "Inactive")}");
        Console.WriteLine($"Created: {CreationDate.ToShortDateString()}");
    }
}

// ============================================
// INHERITANCE: SavingsAccount inherits from Account
// ============================================
public class SavingsAccount : Account, ITransferable
{
    // Auto-implemented properties
    public double InterestRate { get; set; }
    public decimal MinimumBalance { get; set; }

    public SavingsAccount(string accountNumber, Customer accountHolder,
        decimal initialBalance, double interestRate, decimal minimumBalance)
        : base(accountNumber, accountHolder, initialBalance)
    {
        InterestRate = interestRate;
        MinimumBalance = minimumBalance;
    }

    public override decimal CalculateInterest()
    {
        return Balance * (decimal)InterestRate / 100m;
    }

    public override string GetAccountType()
    {
        return "Savings Account";
    }

    public override bool Withdraw(decimal amount)
    {
        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine($"❌ Cannot withdraw. Minimum balance of ${MinimumBalance:N2} required!");
            return false;
        }

        return base.Withdraw(amount);
    }

    public void ApplyMonthlyInterest()
    {
        decimal interest = CalculateInterest();
        Balance += interest;
        Transactions.Add(new Transaction(TransactionType.Interest, interest,
            $"Monthly interest at {InterestRate}%", null, this));
        Console.WriteLine($"✓ Applied interest: ${interest:N2}. New balance: ${Balance:N2}");
    }

    // ITransferable implementation
    public bool CanTransferTo(Account destination)
    {
        return destination != null && destination.IsActive && this.IsActive;
    }

    public decimal GetTransferFee(decimal amount)
    {
        // Savings accounts have no transfer fee
        return 0m;
    }

    public Transaction ExecuteTransfer(Account destination, decimal amount)
    {
        if (!CanTransferTo(destination))
        {
            Console.WriteLine("❌ Transfer not allowed!");
            return null;
        }

        if (Balance - amount < MinimumBalance)
        {
            Console.WriteLine($"❌ Transfer would violate minimum balance requirement!");
            return null;
        }

        if (amount <= 0)
        {
            Console.WriteLine("❌ Transfer amount must be positive!");
            return null;
        }

        Balance -= amount;
        destination.Deposit(amount);

        Transaction trans = new Transaction(TransactionType.Transfer, amount,
            $"Transfer to {destination.AccountNumber}", this, destination);
        Transactions.Add(trans);

        return trans;
    }
}

// ============================================
// INHERITANCE: CheckingAccount inherits from Account
// ============================================
public class CheckingAccount : Account, ITransferable
{
    // Auto-implemented properties
    public decimal OverdraftLimit { get; set; }
    public decimal MonthlyFee { get; set; }
    public int FreeTransactionsPerMonth { get; set; }
    public int TransactionsThisMonth { get; set; }

    public CheckingAccount(string accountNumber, Customer accountHolder,
        decimal initialBalance, decimal overdraftLimit, decimal monthlyFee)
        : base(accountNumber, accountHolder, initialBalance)
    {
        OverdraftLimit = overdraftLimit;
        MonthlyFee = monthlyFee;
        FreeTransactionsPerMonth = 10;
        TransactionsThisMonth = 0;
    }

    public override decimal CalculateInterest()
    {
        // Checking accounts have minimal interest (0.1%)
        return Balance * 0.001m;
    }

    public override string GetAccountType()
    {
        return "Checking Account";
    }

    public override bool Withdraw(decimal amount)
    {
        TransactionsThisMonth++;

        if (Balance - amount < -OverdraftLimit)
        {
            Console.WriteLine($"❌ Withdrawal exceeds overdraft limit of ${OverdraftLimit:N2}!");
            return false;
        }

        Balance -= amount;
        Transactions.Add(new Transaction(TransactionType.Withdrawal, amount,
            "Withdrawal", this, null));

        if (Balance < 0)
        {
            Console.WriteLine($"⚠ Withdrew ${amount:N2}. OVERDRAFT! Balance: ${Balance:N2}");
        }
        else
        {
            Console.WriteLine($"✓ Withdrew ${amount:N2}. Balance: ${Balance:N2}");
        }

        return true;
    }

    public void ChargeMonthlyFee()
    {
        Balance -= MonthlyFee;
        Transactions.Add(new Transaction(TransactionType.Fee, MonthlyFee,
            "Monthly maintenance fee", this, null));
        Console.WriteLine($"✓ Charged monthly fee: ${MonthlyFee:N2}");
    }

    public void ResetMonthlyTransactions()
    {
        TransactionsThisMonth = 0;
        Console.WriteLine("✓ Monthly transaction count reset");
    }

    // ITransferable implementation
    public bool CanTransferTo(Account destination)
    {
        return destination != null && destination.IsActive && this.IsActive;
    }

    public decimal GetTransferFee(decimal amount)
    {
        // Fee after free transactions
        return TransactionsThisMonth > FreeTransactionsPerMonth ? 1.50m : 0m;
    }

    public Transaction ExecuteTransfer(Account destination, decimal amount)
    {
        if (!CanTransferTo(destination))
        {
            Console.WriteLine("❌ Transfer not allowed!");
            return null;
        }

        decimal fee = GetTransferFee(amount);
        decimal totalAmount = amount + fee;

        if (Balance - totalAmount < -OverdraftLimit)
        {
            Console.WriteLine($"❌ Transfer would exceed overdraft limit!");
            return null;
        }

        Balance -= totalAmount;
        destination.Deposit(amount);
        TransactionsThisMonth++;

        if (fee > 0)
        {
            Transactions.Add(new Transaction(TransactionType.Fee, fee,
                "Transfer fee", this, null));
            Console.WriteLine($"⚠ Transfer fee: ${fee:N2}");
        }

        Transaction trans = new Transaction(TransactionType.Transfer, amount,
            $"Transfer to {destination.AccountNumber}", this, destination);
        Transactions.Add(trans);

        return trans;
    }
}

// ============================================
// INHERITANCE: InvestmentAccount inherits from Account
// ============================================
public class InvestmentAccount : Account
{
    // Auto-implemented properties
    public RiskLevel Risk { get; set; }
    public Dictionary<string, int> Holdings { get; set; }
    public decimal CurrentValue { get; set; }

    public InvestmentAccount(string accountNumber, Customer accountHolder,
        decimal initialBalance, RiskLevel riskLevel)
        : base(accountNumber, accountHolder, initialBalance)
    {
        Risk = riskLevel;
        Holdings = new Dictionary<string, int>();
        CurrentValue = initialBalance;
    }

    public override decimal CalculateInterest()
    {
        // Investment returns vary by risk level
        decimal returnRate = Risk switch
        {
            RiskLevel.Low => 0.05m,
            RiskLevel.Medium => 0.08m,
            RiskLevel.High => 0.12m,
            _ => 0.05m
        };
        return Balance * returnRate;
    }

    public override string GetAccountType()
    {
        return $"Investment Account ({Risk} Risk)";
    }

    public override bool Withdraw(decimal amount)
    {
        // Early withdrawal penalty
        decimal penalty = amount * 0.02m;
        decimal totalWithdrawal = amount + penalty;

        if (totalWithdrawal > Balance)
        {
            Console.WriteLine("❌ Insufficient funds including penalty!");
            return false;
        }

        Balance -= totalWithdrawal;
        Transactions.Add(new Transaction(TransactionType.Withdrawal, amount,
            "Withdrawal", this, null));
        Transactions.Add(new Transaction(TransactionType.Fee, penalty,
            "Early withdrawal penalty", this, null));

        Console.WriteLine($"✓ Withdrew ${amount:N2} (Penalty: ${penalty:N2})");
        Console.WriteLine($"New balance: ${Balance:N2}");
        return true;
    }

    public void BuyStock(string symbol, int quantity, decimal pricePerShare)
    {
        decimal totalCost = quantity * pricePerShare;

        if (totalCost > Balance)
        {
            Console.WriteLine("❌ Insufficient funds to buy stock!");
            return;
        }

        Balance -= totalCost;

        if (Holdings.ContainsKey(symbol))
        {
            Holdings[symbol] += quantity;
        }
        else
        {
            Holdings[symbol] = quantity;
        }

        Transactions.Add(new Transaction(TransactionType.StockPurchase, totalCost,
            $"Bought {quantity} shares of {symbol}", this, null));

        Console.WriteLine($"✓ Bought {quantity} shares of {symbol} at ${pricePerShare:N2}/share");
        Console.WriteLine($"Total cost: ${totalCost:N2}");
    }

    public void SellStock(string symbol, int quantity, decimal pricePerShare)
    {
        if (!Holdings.ContainsKey(symbol) || Holdings[symbol] < quantity)
        {
            Console.WriteLine("❌ Insufficient shares to sell!");
            return;
        }

        decimal totalValue = quantity * pricePerShare;
        Balance += totalValue;
        Holdings[symbol] -= quantity;

        if (Holdings[symbol] == 0)
        {
            Holdings.Remove(symbol);
        }

        Transactions.Add(new Transaction(TransactionType.StockSale, totalValue,
            $"Sold {quantity} shares of {symbol}", null, this));

        Console.WriteLine($"✓ Sold {quantity} shares of {symbol} at ${pricePerShare:N2}/share");
        Console.WriteLine($"Total value: ${totalValue:N2}");
    }
}

// ============================================
// Customer class
// ============================================
public class Customer
{
    // Auto-implemented properties
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}"; // Computed property
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string SSN { get; private set; } // Private setter for security
    public List<Account> Accounts { get; set; }

    public Customer(int customerId, string firstName, string lastName,
        DateTime dateOfBirth, string address, string ssn)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Address = address;
        SSN = ssn;
        Accounts = new List<Account>();
    }

    public void OpenAccount(Account account, Bank bank)
    {
        Accounts.Add(account);
        bank.AddAccount(account);
        Console.WriteLine($"✓ {FullName} opened {account.GetAccountType()} #{account.AccountNumber}");
    }

    public void CloseAccount(Account account)
    {
        if (Accounts.Contains(account))
        {
            account.IsActive = false;
            Accounts.Remove(account);
            Console.WriteLine($"✓ Closed account {account.AccountNumber}");
        }
    }

    public decimal GetTotalBalance()
    {
        return Accounts.Sum(a => a.Balance);
    }

    public void DisplayCustomerInfo()
    {
        Console.WriteLine($"\n=== Customer: {FullName} (ID: {CustomerId}) ===");
        Console.WriteLine($"Address: {Address}");
        Console.WriteLine($"Total Accounts: {Accounts.Count}");
        Console.WriteLine($"Total Balance: ${GetTotalBalance():N2}");
        Console.WriteLine("\nAccounts:");
        foreach (var account in Accounts)
        {
            Console.WriteLine($"  - {account.GetAccountType()}: {account.AccountNumber} (${account.Balance:N2})");
        }
    }
}

// ============================================
// Transaction class
// ============================================
public class Transaction
{
    private static int nextId = 1;

    // Auto-implemented properties
    public int TransactionId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Account FromAccount { get; set; }
    public Account ToAccount { get; set; }
    public string Description { get; set; }

    public Transaction(TransactionType type, decimal amount, string description,
        Account fromAccount, Account toAccount)
    {
        TransactionId = nextId++;
        Type = type;
        Amount = amount;
        Description = description;
        FromAccount = fromAccount;
        ToAccount = toAccount;
        Date = DateTime.Now;
    }

    public void DisplayTransaction()
    {
        Console.WriteLine($"[{Date:MM/dd/yyyy HH:mm}] {Type}: ${Amount:N2} - {Description}");
    }
}

// ============================================
// Bank class
// ============================================
public class Bank
{
    // Auto-implemented properties
    public string BankName { get; set; }
    public string BankCode { get; set; }
    public List<Customer> Customers { get; set; }
    public List<Account> Accounts { get; set; }

    public Bank(string bankName, string bankCode)
    {
        BankName = bankName;
        BankCode = bankCode;
        Customers = new List<Customer>();
        Accounts = new List<Account>();
    }

    public void RegisterCustomer(Customer customer)
    {
        Customers.Add(customer);
        Console.WriteLine($"✓ Registered customer: {customer.FullName}");
    }

    public void AddAccount(Account account)
    {
        Accounts.Add(account);
    }

    public bool TransferFunds(Account from, Account to, decimal amount)
    {
        if (from is ITransferable transferable)
        {
            Transaction trans = transferable.ExecuteTransfer(to, amount);
            if (trans != null)
            {
                Console.WriteLine($"✓ Transfer completed: ${amount:N2} from {from.AccountNumber} to {to.AccountNumber}");
                return true;
            }
        }
        return false;
    }

    public Account GetAccountByNumber(string accountNumber)
    {
        return Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }

    public void GenerateBankStatement(Account account, DateTime startDate, DateTime endDate)
    {
        Console.WriteLine($"\n╔═══════════════════════════════════════════╗");
        Console.WriteLine($"║        {BankName} - Bank Statement        ║");
        Console.WriteLine($"╚═══════════════════════════════════════════╝");
        Console.WriteLine($"Account: {account.AccountNumber}");
        Console.WriteLine($"Period: {startDate:MM/dd/yyyy} to {endDate:MM/dd/yyyy}");
        Console.WriteLine($"Current Balance: ${account.Balance:N2}\n");

        var transactions = account.GetTransactionHistory()
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderBy(t => t.Date);

        Console.WriteLine("Transactions:");
        foreach (var trans in transactions)
        {
            trans.DisplayTransaction();
        }
    }

    public void DisplayBankInfo()
    {
        Console.WriteLine($"\n=== {BankName} ({BankCode}) ===");
        Console.WriteLine($"Total Customers: {Customers.Count}");
        Console.WriteLine($"Total Accounts: {Accounts.Count}");
        Console.WriteLine($"Total Assets: ${Accounts.Sum(a => a.Balance):N2}");
    }
}

// ============================================
// INTERFACE: ITransferable
// ============================================
public interface ITransferable
{
    bool CanTransferTo(Account destination);
    decimal GetTransferFee(decimal amount);
    Transaction ExecuteTransfer(Account destination, decimal amount);
}

// ============================================
// ENUMS
// ============================================
public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer,
    Interest,
    Fee,
    StockPurchase,
    StockSale
}

public enum RiskLevel
{
    Low,
    Medium,
    High
}

// ============================================
// MAIN PROGRAM
// ============================================
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║      BANKING SYSTEM - OOP TUTORIAL        ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");

        // ========================================
        // 1. CREATE BANK
        // ========================================
        Console.WriteLine("═══ 1. Creating Bank ═══");
        Bank firstNational = new Bank("First National Bank", "FNB001");
        Console.WriteLine($"✓ Created {firstNational.BankName}\n");

        // ========================================
        // 2. CREATE CUSTOMERS
        // ========================================
        Console.WriteLine("═══ 2. Creating Customers ═══");
        Customer john = new Customer(1001, "John", "Smith",
            new DateTime(1985, 5, 15), "123 Main St", "***-**-1234");
        Customer sarah = new Customer(1002, "Sarah", "Johnson",
            new DateTime(1990, 8, 22), "456 Oak Ave", "***-**-5678");

        firstNational.RegisterCustomer(john);
        firstNational.RegisterCustomer(sarah);
        Console.WriteLine();

        // ========================================
        // 3. CREATE DIFFERENT ACCOUNT TYPES
        // ========================================
        Console.WriteLine("═══ 3. Creating Different Account Types (Inheritance) ═══");

        SavingsAccount johnSavings = new SavingsAccount("SAV-001", john, 5000m, 2.5, 500m);
        CheckingAccount johnChecking = new CheckingAccount("CHK-001", john, 2000m, 500m, 10m);
        InvestmentAccount johnInvestment = new InvestmentAccount("INV-001", john, 10000m, RiskLevel.Medium);

        john.OpenAccount(johnSavings, firstNational);
        john.OpenAccount(johnChecking, firstNational);
        john.OpenAccount(johnInvestment, firstNational);

        SavingsAccount sarahSavings = new SavingsAccount("SAV-002", sarah, 8000m, 2.5, 500m);
        sarah.OpenAccount(sarahSavings, firstNational);
        Console.WriteLine();

        // ========================================
        // 4. POLYMORPHISM - Different account types treated as Account
        // ========================================
        Console.WriteLine("═══ 4. Polymorphism - Treating All as Account Type ═══");
        List<Account> allAccounts = new List<Account> { johnSavings, johnChecking, johnInvestment, sarahSavings };

        foreach (var account in allAccounts)
        {
            Console.WriteLine($"{account.GetAccountType()}: ${account.Balance:N2}");
        }
        Console.WriteLine();

        // ========================================
        // 5. METHOD OVERRIDING - Deposit
        // ========================================
        Console.WriteLine("═══ 5. Method Overriding - Deposits ═══");
        johnSavings.Deposit(1000m);
        johnChecking.Deposit(500m);
        Console.WriteLine();

        // ========================================
        // 6. METHOD OVERRIDING - Withdrawal with different rules
        // ========================================
        Console.WriteLine("═══ 6. Different Withdrawal Rules per Account Type ═══");

        Console.WriteLine("\n--- Savings Account (minimum balance enforced) ---");
        johnSavings.Withdraw(5800m); // Should fail - would go below minimum

        Console.WriteLine("\n--- Checking Account (overdraft allowed) ---");
        johnChecking.Withdraw(2800m); // Should work - overdraft allowed

        Console.WriteLine("\n--- Investment Account (penalty applied) ---");
        johnInvestment.Withdraw(1000m); // Should work with penalty
        Console.WriteLine();

        // ========================================
        // 7. SPECIFIC ACCOUNT BEHAVIORS
        // ========================================
        Console.WriteLine("═══ 7. Account-Specific Methods ═══");

        Console.WriteLine("\n--- Savings: Apply Monthly Interest ---");
        johnSavings.ApplyMonthlyInterest();

        Console.WriteLine("\n--- Checking: Charge Monthly Fee ---");
        johnChecking.ChargeMonthlyFee();

        Console.WriteLine("\n--- Investment: Buy/Sell Stocks ---");
        johnInvestment.BuyStock("AAPL", 10, 150m);
        johnInvestment.BuyStock("GOOGL", 5, 2800m);
        johnInvestment.SellStock("AAPL", 5, 155m);
        Console.WriteLine();

        // ========================================
        // 8. INTERFACE IMPLEMENTATION - Transfers
        // ========================================
        Console.WriteLine("═══ 8. Interface (ITransferable) - Fund Transfers ═══");

        Console.WriteLine("\n--- Transfer from Savings to Checking ---");
        firstNational.TransferFunds(johnSavings, johnChecking, 500m);

        Console.WriteLine("\n--- Transfer between customers ---");
        firstNational.TransferFunds(johnChecking, sarahSavings, 200m);
        Console.WriteLine();

        // ========================================
        // 9. POLYMORPHISM WITH INTERFACE
        // ========================================
        Console.WriteLine("═══ 9. Interface Polymorphism ═══");
        List<ITransferable> transferableAccounts = new List<ITransferable> { johnSavings, johnChecking };

        foreach (var transferable in transferableAccounts)
        {
            Account acc = transferable as Account;
            Console.WriteLine($"{acc?.GetAccountType()}: Transfer fee for $100 = ${transferable.GetTransferFee(100):N2}");
        }
        Console.WriteLine();

        // ========================================
        // 10. CALCULATE INTEREST (Abstract method implementation)
        // ========================================
        Console.WriteLine("═══ 10. Calculate Interest (Abstract Method) ═══");
        foreach (var account in allAccounts)
        {
            decimal interest = account.CalculateInterest();
            Console.WriteLine($"{account.GetAccountType()}: Potential interest = ${interest:N2}");
        }
        Console.WriteLine();

        // ========================================
        // 11. CUSTOMER INFORMATION
        // ========================================
        Console.WriteLine("═══ 11. Customer Account Summary ═══");
        john.DisplayCustomerInfo();
        sarah.DisplayCustomerInfo();
        Console.WriteLine();

        // ========================================
        // 12. TRANSACTION HISTORY
        // ========================================
        Console.WriteLine("═══ 12. Transaction History ═══");
        var transactions = johnChecking.GetTransactionHistory();
        Console.WriteLine($"\nTransaction history for {johnChecking.AccountNumber}:");
        foreach (var trans in transactions.Take(5))
        {
            trans.DisplayTransaction();
        }
        Console.WriteLine();

        // ========================================
        // 13. BANK STATEMENT
        // ========================================
        Console.WriteLine("═══ 13. Generate Bank Statement ═══");
        firstNational.GenerateBankStatement(johnSavings,
            DateTime.Now.AddDays(-30), DateTime.Now);
        Console.WriteLine();

        // ========================================
        // 14. BANK OVERVIEW
        // ========================================
        Console.WriteLine("═══ 14. Bank Overview ═══");
        firstNational.DisplayBankInfo();

        // ========================================
        // SUMMARY
        // ========================================
        Console.WriteLine("\n╔════════════════════════════════════════════╗");
        Console.WriteLine("║        OOP CONCEPTS DEMONSTRATED          ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.WriteLine("✓ ENCAPSULATION: Private fields, protected balance");
        Console.WriteLine("✓ INHERITANCE: SavingsAccount/CheckingAccount/InvestmentAccount → Account");
        Console.WriteLine("✓ POLYMORPHISM: Different withdraw/deposit behavior per account type");
        Console.WriteLine("✓ ABSTRACTION: Abstract Account class with abstract methods");
        Console.WriteLine("✓ COMPOSITION: Bank contains Customers and Accounts");
        Console.WriteLine("✓ INTERFACES: ITransferable for transfer-capable accounts");
        Console.WriteLine("✓ ENUMS: TransactionType and RiskLevel");
        Console.WriteLine("✓ COLLECTIONS: Lists and Dictionaries for managing data");
        Console.WriteLine("✓ PROTECTED: Protected set for Balance, protected transactions list");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}