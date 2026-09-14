using System;

namespace laba1.Models
{
    public class BankAccount
    {
        
        public string AccountNumber { get; set; }
        public string AccountOwner { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } = true; 
        public bool IsDeleted { get; set; } = false;

       
        public BankAccount()
        {
        }

        
        public BankAccount(string accountNumber, string accountOwner, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            AccountOwner = accountOwner;
            Balance = initialBalance;
            IsActive = true;
        }

        public bool RemouveAccount()
        {
            if (Balance == 0 || Balance < 0)
            {
                IsDeleted = true;
                return true;
            }
            return false;
        }

        public bool RestoreAccount()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                return true;
            }
            return false;
        }

        public bool Deposit(decimal amount)
        {
            if (amount > 0 && IsActive)
            {
                Balance += amount;
                return true;
            }
            return false;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= Balance && IsActive)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public bool EditOwner(string newOwner)
        {
            if (!string.IsNullOrWhiteSpace(newOwner))
            {
                AccountOwner = newOwner;
                return true;
            }
            return false;
        }

        public bool FreezeAccount()
        {
            if (IsActive)
            {
                IsActive = false;
                return true;
            }
            return false;
        }

        public bool UnfreezeAccount()
        {
            if (!IsActive)
            {
                IsActive = true;
                return true;
            }
            return false;
        }

        public bool Transfer(BankAccount targetAccount, decimal amount)
        {
            if (targetAccount != null && Withdraw(amount))
            {
                targetAccount.Deposit(amount);
                return true;
            }
            return false;
        }
    }
}