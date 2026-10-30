
﻿using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;
using Microsoft.EntityFrameworkCore;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Lab1.Model.ModelsDB;
using Ninject.Modules;


namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Регистрация зависимостей бизнес-логики в контейнере Ninject.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        /// <summary>
        /// Регистрация зависимостей бизнес-логики в контейнере Ninject.
        /// </summary>
        public override void Load()
        {
            Bind<IRepository<BankAccount>>()
                .To<DapperRepository<BankAccount>>()
                .InSingletonScope()
                .WithConstructorArgument("connectionString", DbConfig.ConnectionString);


            /// Регистрация сервиса управления банковскими счетами
            Bind<IAccountService>().To<AccountService>();

            /// Регистрация сервиса переводов
            Bind<ITransferService>().To<TransferService>();
            /// Регистрация других сервисов бизнес-логики при необходимости
            Bind<Logic>().ToSelf();

        }
    }
}
