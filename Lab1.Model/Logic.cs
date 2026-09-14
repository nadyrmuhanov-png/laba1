using laba1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab1.Model
{
    internal class Logic
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

        public List<BankAccount> GetActiveAccount() => Accounts.Where(x => x.IsActive).ToList();

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

        public void EditAccountOwner(string accountNumber, string newOwner)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.EditOwner(newOwner);
            }
        }
    }
}
