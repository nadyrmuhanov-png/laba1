using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.Model.ModelsDB
{
    public class BankAccount : IDomainObject
    {
        [Key]
        [Column("id_bankAccount")]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string number { get; set; } = string.Empty;

        public int id_type { get; set; }

        public int id_owner { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_create { get; set; } = DateTime.Today;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal balance { get; set; } = 0m;

        public bool isActiv { get; set; } = true;

        public bool isDeleted { get; set; } = false;

        [ForeignKey(nameof(id_type))]
        public virtual Type_BankAccount? Type_BankAccount { get; set; }

        [ForeignKey(nameof(id_owner))]
        public virtual Account? Account { get; set; }


        /// <summary>
        /// Замораживает банковский счёт, если он активен. После заморозки счёт не может быть использован для операций до его разморозки.
        /// </summary>
        /// <returns>bool</returns>
        public bool FreezeAccount()
        {
            if (isActiv)
            {
                isActiv = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Размораживает банковский счёт, если он заморожен. После разморозки счёт может быть использован для операций.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        public bool UnfreezeAccount()
        {
            if (!isActiv)
            {
                isActiv = true;
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
                Account.full_name = newOwner;
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
            if (amount > 0 && amount <= balance && isActiv)
            {
                balance -= amount;
                return true;
            }
            return false;
        }

    }
}
