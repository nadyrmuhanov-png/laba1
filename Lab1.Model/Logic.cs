using laba1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace laba1.Models
{
    public class Logic
    {
        public List<BankAccount> Accounts { get; } = new List<BankAccount>();

        public Logic()
        {
            
            Accounts.Add(new BankAccount("40817810000000000001", "Алексей Смирнов", 45000m));
            Accounts.Add(new BankAccount("40817810000000000002", "Ирина Кузнецова", 128000m));
            Accounts.Add(new BankAccount("40817810000000000003", "Дмитрий Соколов", 3500m));
            Accounts.Add(new BankAccount("40817810000000000004", "Ольга Попова", 92000m));

        }

        public void AddAccount(BankAccount account) => Accounts.Add(account);

        public BankAccount? GetAccount(string accountNumber) => Accounts.Find(a => a.AccountNumber == accountNumber);

        public List<BankAccount> GetActiveAccount() => Accounts.Where(x => x.IsActive || !x.IsDeleted).ToList();

        public List<BankAccount> GetDeletedAccount() => Accounts.Where(x => x.IsDeleted).ToList();

        public void FreezeAccountNumber(string accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.FreezeAccount();
            }
        }

        public void UnfreezeAccountNumber(string accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.UnfreezeAccount();
            }
        }

        public bool EditAccountOwner(string accountNumber, string newOwner)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.EditOwner(newOwner);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Transfer(BankAccount targetAccount, BankAccount senderAccount, decimal amount)
        {
            if (senderAccount.Withdraw(amount))
            {
                if (targetAccount.Deposit(amount))
                    return true;
                else
                {
                    senderAccount.Deposit(amount);
                    return false;

                }

            }
            else return false;          

        }
    }
}
