
﻿using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;

namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Фасад бизнес-логики для слоя представления.
    /// Делегирует операции управления счетами и переводы специализированным сервисам.
    /// </summary>
    public class Logic
    {
        private readonly AccountService accountService;
        private readonly TransferService transferService;

        public Logic(AccountService accountService, TransferService transferService)
        {
            this.accountService = accountService;
            this.transferService = transferService;
        }

        public List<BankAccount> GetAccounts(bool deleted) =>
            accountService.GetAccounts(deleted);

        public List<BankAccount> GetActiveAccounts() =>
            accountService.GetActiveAccounts();

        public BankAccount? GetById(int id) =>
            accountService.GetById(id);

        public BankAccount? FindByNumber(string? number, bool includeDeleted = false) =>
            accountService.FindByNumber(number, includeDeleted);

        public OperationResult AddAccount(
            string? number, int ownerId, int typeId, decimal initialBalance) =>
            accountService.AddAccount(number, ownerId, typeId, initialBalance);

        public OperationResult ChangeOwner(int accountId, int newOwnerId) =>
            accountService.ChangeOwner(accountId, newOwnerId);

        public OperationResult DeleteAccount(int accountId) =>
            accountService.DeleteAccount(accountId);

        public OperationResult RestoreAccount(int accountId) =>
            accountService.RestoreAccount(accountId);

        public OperationResult ToggleFreeze(int accountId) =>
            accountService.ToggleFreeze(accountId);

        public OperationResult Transfer(int senderId, string? targetNumber, decimal amount) =>
            transferService.Transfer(senderId, targetNumber, amount);
    }
}

