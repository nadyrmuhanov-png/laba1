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
        /// <summary>
        /// Добавляет новый банковский счёт в список.
        /// </summary>
        /// <param name="account">Банковский счёт для добавления</param>
        public void AddAccount(BankAccount account) => Accounts.Add(account);

        public BankAccount? GetAccount(string accountNumber) => Accounts.Find(a => a.AccountNumber == accountNumber);

        public List<BankAccount> GetActiveAccount() => Accounts.Where(x => x.IsActive || !x.IsDeleted).ToList();

        public List<BankAccount> GetDeletedAccount() => Accounts.Where(x => x.IsDeleted).ToList();

        /// <summary>
        /// Замораживает банковский счёт с указанным номером, если он существует в списке.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта для заморозки</param>
        public void FreezeAccountNumber(string accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.FreezeAccount();
            }
        }
        
        /// <summary>
        /// Размораживает банковский счёт с указанным номером, если он существует в списке.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта для разморозки</param>
        public void UnfreezeAccountNumber(string accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.UnfreezeAccount();
            }
        }
        /// <summary>
        /// Изменяет владельца банковского счёта с указанным номером, если он существует в списке.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта для изменения владельца</param>
        /// <param name="newOwner">Новый владелец счёта</param>
        /// <returns></returns>
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
        /// <summary>
        /// Удаляет банковский счёт с указанным номером, если он существует в списке.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта для удаления</param>
        /// <returns></returns>
        public bool RemoveAccount(string accountNumber)
        {
            var account = GetAccount(accountNumber);
            return account != null && account.RemouveAccount();
        }

        /// <summary>
        /// Восстанавливает удалённый банковский счёт с указанным номером, если он существует в списке.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта для восстановления</param>
        /// <returns></returns>
        public bool RestoreAccountByNumber(string accountNumber)
        {
            var account = Accounts.Find(a => a.AccountNumber == accountNumber);
            return account != null && account.RestoreAccount();
        }
        /// <summary>
        /// Переводит указанную сумму с одного банковского счёта на другой, если оба счёта существуют в списке и имеют достаточный баланс.
        /// </summary>
        /// <param name="senderAccount">Счёт отправителя</param>
        /// <param name="targetAccount">Счёт получателя</param>
        /// <param name="amount">Сумма перевода</param>
        /// <returns></returns>
        public bool Transfer(BankAccount senderAccount, BankAccount targetAccount, decimal amount)
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
