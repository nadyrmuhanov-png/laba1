using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;
using Microsoft.EntityFrameworkCore;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Модуль конфигурации зависимостей Ninject для слоя бизнес-логики.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        /// <summary>
        /// Конфигурирует внедрение зависимостей для репозиториев банковских счетов с использованием выбранной
        /// реализации.
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрана неподдерживаемая реализация репозитория.</exception>
        public override void Load()
        {
            string selection = "EntityFramework"; // Выбор реализации репозитория: "EntityFramework" или "Dapper"

            if (selection == "EntityFramework")
            {
                // Связываем интерфейс репозитория с EntityFramework-реализацией
                Bind<DbContext>().To<DBContext>().InSingletonScope();
                Bind<IRepository<BankAccount>>().To<EntityRepository<BankAccount>>().InSingletonScope();
            }
            else if (selection == "Dapper")
            {
                // Связываем интерфейс репозитория с Dapper-реализацией
                Bind<IRepository<BankAccount>>().To<DapperBankAccountRepository>().InSingletonScope()
                    .WithConstructorArgument("connectionString", DbConfig.ConnectionString);
            }
            else
            {
                throw new InvalidOperationException("Неверный выбор реализации репозитория.");
            }
            
            
        }
    }
}
