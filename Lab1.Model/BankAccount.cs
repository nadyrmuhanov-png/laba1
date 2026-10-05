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

        /// <summary>
        /// Инициализирует новый экземпляр класса BankAccount с указанными параметрами.
        /// </summary>
        public BankAccount()
        {
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса BankAccount с указанными параметрами.
        /// </summary>
        public interface IDomainObject
        {
            int Id { get; set; }
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса BankAccount с указанными параметрами.
        /// </summary>
        /// <param name="accountNumber">Номер банковского счёта</param>
        /// <param name="accountOwner">Владелец банковского счёта</param>
        /// <param name="initialBalance">Начальный баланс</param>
        public BankAccount(string accountNumber, string accountOwner, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            AccountOwner = accountOwner;
            Balance = initialBalance;
            IsActive = true;
        }

        /// <summary>
        /// Удаляет банковский счёт, если его баланс равен нулю или меньше нуля.
        /// </summary>
        /// <returns></returns>
        public bool RemouveAccount()
        {
            if (Balance == 0 || Balance < 0)
            {
                IsDeleted = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Восстанавливает удалённый банковский счёт, если он был удалён.
        /// </summary>
        /// <returns></returns>
        public bool RestoreAccount()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Пополняет баланс банковского счёта на указанную сумму, если сумма положительная и счёт активен.
        /// </summary>
        /// <param name="amount">Сумма для пополнения</param>
        /// <returns></returns>
        public bool Deposit(decimal amount)
        {
            if (amount > 0 && IsActive)
            {
                Balance += amount;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Снимает указанную сумму с баланса банковского счёта, если сумма положительная, не превышает текущий баланс и счёт активен.
        /// </summary>
        /// <param name="amount">Сумма для снятия</param>
        /// <returns></returns>
        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= Balance && IsActive)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Изменяет владельца банковского счёта на указанного нового владельца, если новый владелец не пустой и не состоит только из пробелов.
        /// </summary>
        /// <param name="newOwner">Новый владелец счёта</param>
        /// <returns></returns>
        public bool EditOwner(string newOwner)
        {
            if (!string.IsNullOrWhiteSpace(newOwner))
            {
                AccountOwner = newOwner;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Замораживает банковский счёт, если он активен. После заморозки счёт не может быть использован для операций до его разморозки.
        /// </summary>
        /// <returns></returns>
        public bool FreezeAccount()
        {
            if (IsActive)
            {
                IsActive = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Размораживает банковский счёт, если он заморожен. После разморозки счёт может быть использован для операций.
        /// </summary>
        /// <returns></returns>
        public bool UnfreezeAccount()
        {
            if (!IsActive)
            {
                IsActive = true;
                return true;
            }
            return false;
        }

       
    }
}