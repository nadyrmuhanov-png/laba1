using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;

namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Отвечает за операции управления банковскими счетами:
    /// получение, поиск, создание, изменение владельца, удаление,
    /// восстановление и изменение состояния счёта.
    /// </summary>
    public class AccountService
    {
        private readonly IRepository<BankAccount> repository;

        public AccountService(IRepository<BankAccount> repository)
        {
            this.repository = repository;
        }

        public List<BankAccount> GetAccounts(bool deleted) =>
            repository.ReadAll().Where(a => a.isDeleted == deleted).ToList();

        public List<BankAccount> GetActiveAccounts() =>
            repository.ReadAll().Where(a => a.isActiv && !a.isDeleted).ToList();

        public BankAccount? GetById(int id) => repository.ReadById(id);

        public BankAccount? FindByNumber(string? number, bool includeDeleted = false) =>
            repository.ReadAll().FirstOrDefault(a =>
                a.number.Trim() == (number ?? string.Empty).Trim() &&
                (includeDeleted || !a.isDeleted));
        /// <summary>
        /// Добавляет новый банковский счёт.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="ownerId"></param>
        /// <param name="typeId"></param>
        /// <param name="initialBalance"></param>
        /// <returns>
        /// Результат операции добавления счёта.
        /// </returns>
        public OperationResult AddAccount(
            string? number, int ownerId, int typeId, decimal initialBalance)
        {
            number = (number ?? string.Empty).Trim();

            if (number.Length == 0)
                return OperationResult.Fail("Введите номер счёта.");
            if (number.Length > 20 || !number.All(char.IsDigit))
                return OperationResult.Fail("Номер счёта — только цифры, не длиннее 20 символов.");
            if (ownerId <= 0)
                return OperationResult.Fail("ID владельца должен быть положительным числом.");
            if (initialBalance < 0)
                return OperationResult.Fail("Начальный баланс не может быть отрицательным.");

            return Execute(() =>
            {
                if (FindByNumber(number, includeDeleted: true) != null)
                    return OperationResult.Fail("Счёт с таким номером уже существует.");

                repository.Add(new BankAccount
                {
                    number = number,
                    id_owner = ownerId,
                    id_type = typeId > 0 ? typeId : 1,
                    balance = initialBalance,
                    date_create = DateTime.Today,
                    isActiv = true,
                    isDeleted = false
                });

                return OperationResult.Ok($"Счёт {number} добавлен в базу данных.");
            });
        }

        /// <summary>
        /// Добавляет новый банковский счёт.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newOwnerId"></param>
        /// <returns></returns>
        public OperationResult ChangeOwner(int accountId, int newOwnerId)
        {
            if (newOwnerId <= 0)
                return OperationResult.Fail("ID нового владельца должен быть положительным числом.");

            return Execute(() =>
            {
                var account = repository.ReadById(accountId);
                if (account == null)
                    return OperationResult.Fail("Счёт не найден.");

                account.id_owner = newOwnerId;
                repository.Update(account);
                return OperationResult.Ok("Владелец счёта обновлён.");
            });
        }

        /// <summary>
        /// Удаляет банковский счёт.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public OperationResult DeleteAccount(int accountId)
        {
            return Execute(() =>
            {
                var account = repository.ReadById(accountId);
                if (account == null)
                    return OperationResult.Fail("Счёт не найден.");
                if (account.isDeleted)
                    return OperationResult.Fail("Счёт уже удалён.");
                if (account.balance != 0)
                    return OperationResult.Fail("Нельзя удалить счёт с ненулевым балансом.");

                account.isDeleted = true;
                account.isActiv = false;
                repository.Update(account);
                return OperationResult.Ok($"Счёт {account.number} помечен как удалённый.");
            });
        }


        /// <summary>
        /// Восстанавливает удалённый банковский счёт.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>
        /// Результат операции восстановления счёта.
        /// </returns>

        public OperationResult RestoreAccount(int accountId)
        {
            return Execute(() =>
            {
                var account = repository.ReadById(accountId);
                if (account == null)
                    return OperationResult.Fail("Счёт не найден.");
                if (!account.isDeleted)
                    return OperationResult.Fail("Этот счёт не удалён, восстанавливать нечего.");

                account.isDeleted = false;
                account.isActiv = true;
                repository.Update(account);
                return OperationResult.Ok($"Счёт {account.number} восстановлен.");
            });
        }


        /// <summary>
        /// Переключает состояние заморозки банковского счёта.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>
        /// Результат операции переключения состояния заморозки счёта.
        /// </returns>
        public OperationResult ToggleFreeze(int accountId)
        {
            return Execute(() =>
            {
                var account = repository.ReadById(accountId);
                if (account == null)
                    return OperationResult.Fail("Счёт не найден.");
                if (account.isDeleted)
                    return OperationResult.Fail("Счёт удалён. Сначала восстановите его.");

                bool wasActive = account.isActiv;
                if (wasActive)
                    account.FreezeAccount();
                else
                    account.UnfreezeAccount();

                repository.Update(account);
                return OperationResult.Ok(wasActive
                    ? $"Счёт {account.number} заморожен."
                    : $"Счёт {account.number} разморожен.");
            });
        }

        /// <summary>
        /// Выполняет указанное действие и обрабатывает возможные исключения.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>
        /// Результат выполнения действия.
        /// </returns>

        private static OperationResult Execute(Func<OperationResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                string root = ex.GetBaseException().Message;
                if (root.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
                    return OperationResult.Fail("Владельца или типа счёта с таким ID не существует.");

                return OperationResult.Fail("Ошибка доступа к базе данных: " + root);
            }
        }
    }
}
