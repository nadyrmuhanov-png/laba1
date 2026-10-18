using Lab1.Model.ModelsDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lab1.Model
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
            
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DBContext"/> с указанными параметрами конфигурации.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        /// <summary>
        /// Получает или устанавливает набор ролей.
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        /// <summary>
        /// Получает или устанавливает набор типов транзакций.
        /// </summary>
        public DbSet<Type_transaction> Type_transactions { get; set; }
        /// <summary>
        /// Получает или устанавливает набор типов банковских счетов.
        /// </summary>
        public DbSet<Type_BankAccount> Type_BankAccounts { get; set; }
        /// <summary>
        /// Получает или устанавливает набор банковских счетов.
        /// </summary>
        public DbSet<BankAccount> BankAccount { get; set; }
        /// <summary>
        /// Получает или устанавливает набор исторических транзакций.
        /// </summary>
        public DbSet<HistoryTransaction> HistoryTransactions { get; set; }
        /// <summary>
        /// Получает или устанавливает набор транзакций.
        /// </summary>
        /// <param name="optionsBuilder">Строитель параметров конфигурации.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=dbDataBank;Trusted_Connection=True;Persist Security Info=False;Encrypt=True;TrustServerCertificate=True;");
        }
    }
}
