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

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Type_transaction> Type_transactions { get; set; }
        public DbSet<Type_BankAccount> Type_BankAccounts { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<HistoryTransaction> HistoryTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                //Для подключения бд изменить Server=paymch\\SQLEXPRESS на ядро которое установлено на пк
                .UseSqlServer("Server=paymch\\SQLEXPRESS;Database=dbDataBank;Trusted_Connection=True;Persist Security Info=False;Encrypt=True;TrustServerCertificate=true;");
        }
    }
}
