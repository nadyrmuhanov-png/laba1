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
        public override void Load()
        {
            // Связываем интерфейс репозитория с EntityFramework-реализацией
            //Bind<DbContext>().To<DBContext>().InSingletonScope();
            //Bind<IRepository<BankAccount>>().To<EntityRepository<BankAccount>>().InSingletonScope();
            Bind<IRepository<BankAccount>>().To<DapperRepository<BankAccount>>().InSingletonScope().WithConstructorArgument("connectionString",DbConfig.ConnectionString);
            
        }
    }
}
