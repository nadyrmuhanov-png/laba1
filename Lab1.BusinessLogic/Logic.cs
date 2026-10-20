using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;

namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Результат выполнения операции, содержащий признак успешности и сопутствующее сообщение.
    /// <param name="Success">Признак успешности операции: true — успешно, false — неуспешно.</param>
    /// <param name="Message">Сопутствующее сообщение с деталями или описанием ошибки.</param>
    public record OperationResult(bool Success, string Message)
    {
        public static OperationResult Ok(string message = "") => new(true, message);
        public static OperationResult Fail(string message) => new(false, message);
    }

    /// <summary>
    /// Слой бизнес-логики. Хранит правила работы со счетами, а данные берёт из репозитория.
    /// Какой это репозиторий (Dapper или EF) — Logic не знает, она работает с IRepository.
    /// </summary>
    public class Logic
    {
        /// <summary>
        /// Репозиторий для управления сущностями BankAccount.
        /// </summary>
       
        private readonly IRepository<BankAccount> repository;

       
       

        /// <summary>Любой репозиторий, реализующий IRepository, можно передать снаружи.</summary>
        public Logic(IRepository<BankAccount> repository)
        {
            this.repository = repository;
        }

       

        /// <summary>
        /// Возвращает список банковских счетов, у которых свойство isDeleted соответствует заданному значению.
        /// </summary>
        /// <param name="deleted">true — вернуть удалённые счета; false — вернуть активные счета.</param>
        /// <returns>Список объектов BankAccount, отфильтрованный по признаку isDeleted.</returns>
        public List<BankAccount> GetAccounts(bool deleted) =>
            repository.ReadAll().Where(a => a.isDeleted == deleted).ToList();

        /// <summary>Только активные (не заморожены и не удалены).</summary>
        public List<BankAccount> GetActiveAccounts() =>
            repository.ReadAll().Where(a => a.isActiv && !a.isDeleted).ToList();

        public BankAccount? GetById(int id) => repository.ReadById(id);

        public BankAccount? FindByNumber(string? number, bool includeDeleted = false) =>
            repository.ReadAll().FirstOrDefault(a =>
                a.number.Trim() == (number ?? string.Empty).Trim() && (includeDeleted || !a.isDeleted));

        /// <summary>
        /// Создаёт и сохраняет новый банковский счёт в репозитории и возвращает результат операции.
        /// </summary>
      
        /// <param name="number">Номер счёта; только цифры, не длиннее 20 символов, не может быть пустым.</param>
        /// <param name="ownerId">Идентификатор владельца счёта; должен быть положительным.</param>
        /// <param name="typeId">Идентификатор типа счёта; при значении ≤ 0 используется тип по умолчанию (1 — Текущий).</param>
        /// <param name="initialBalance">Начальный баланс счёта; неотрицательное значение.</param>
        /// <returns>OperationResult, указывающий успех или причину отказа; при успешном добавлении содержит сообщение о
        /// созданном счёте.</returns>

        public OperationResult AddAccount(string? number, int ownerId, int typeId, decimal initialBalance)
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
                    id_type = typeId > 0 ? typeId : 1,   // 1 = «Текущий»
                    balance = initialBalance,
                    date_create = DateTime.Today,
                    isActiv = true,
                    isDeleted = false
                });
                return OperationResult.Ok($"Счёт {number} добавлен в базу данных.");
            });
        }
        /// <summary>
        ///  Обновляет владельца указанного счёта.
        /// </summary>
        /// <param name="accountId">Идентификатор счёта для обновления.</param>
        /// <param name="newOwnerId">Идентификатор нового владельца; должен быть положительным числом.</param>
        /// <returns>OperationResult, указывающий на успешное обновление или ошибку с сообщением (например, счёт не найден или
        /// некорректный идентификатор нового владельца).</returns>
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

        /// <summary>«Мягкое» удаление: строка остаётся в БД с флагом isDeleted.</summary>
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
        /// Восстанавливает удалённый счёт: снимает флаг удаления, активирует счёт и сохраняет изменения в репозитории.
        /// </summary>
        /// <remarks>При успешном восстановлении устанавливает account.isDeleted = false, account.isActiv
        /// = true и вызывает repository.Update(account).</remarks>
        /// <param name="accountId">Идентификатор счёта для восстановления.</param>
        /// <returns>OperationResult с результатом операции: Ok с сообщением о восстановлении при успешном восстановлении; Fail
        /// при отсутствии счёта или если счёт не был помечен как удалённый.</returns>
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
        /// Переключает состояние заморозки указанного счёта.
        /// </summary>
        /// <param name="accountId">Идентификатор счёта для переключения состояния заморозки.</param>
        /// <returns>OperationResult с результатом операции: Ok с сообщением о переключении состояния при успешном выполнении; Fail
        /// при отсутствии счёта или если счёт удалён.</returns>
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
                if (wasActive) account.FreezeAccount();
                else account.UnfreezeAccount();

                repository.Update(account);
                return OperationResult.Ok(wasActive
                    ? $"Счёт {account.number} заморожен."
                    : $"Счёт {account.number} разморожен.");
            });
        }
        /// <summary>
        /// Переводит указанную сумму со счёта отправителя на счёт получателя по номеру.
        /// </summary>
      
        /// <param name="senderId">Идентификатор счёта отправителя.</param>
        /// <param name="targetNumber">Номер счёта получателя; если null или счёт не найден, перевод отклоняется.</param>
        /// <param name="amount">Сумма перевода. Должна быть положительной.</param>
        /// <returns>OperationResult с результатом операции: успех с подтверждением суммы или неудача с причиной (недостаточно
        /// средств, счёт не найден/удалён, заморожен, попытка перевода самому себе и т.п.).</returns>
        public OperationResult Transfer(int senderId, string? targetNumber, decimal amount)
        {
            if (amount <= 0)
                return OperationResult.Fail("Сумма перевода должна быть положительной.");

            return Execute(() =>
            {
                var sender = repository.ReadById(senderId);
                if (sender == null || sender.isDeleted)
                    return OperationResult.Fail("Счёт-отправитель не найден.");

                var target = FindByNumber(targetNumber);
                if (target == null)
                    return OperationResult.Fail("Счёт-получатель не найден.");
                if (target.Id == sender.Id)
                    return OperationResult.Fail("Нельзя перевести самому себе.");
                if (!sender.isActiv || !target.isActiv)
                    return OperationResult.Fail("Один из счетов заморожен. Перевод невозможен.");
                if (sender.balance < amount)
                    return OperationResult.Fail("Недостаточно средств на счёте.");

                sender.Withdraw(amount);
                target.balance += amount;
                repository.Update(sender);
                repository.Update(target);
                return OperationResult.Ok($"Перевод на сумму {amount:C2} выполнен.");
            });
        }

       

       /// <summary>
       /// Выполняет делегат и преобразует исключения в соответствующий OperationResult, обрабатывая ошибки доступа к
       /// базе данных.
       /// </summary>
       /// <param name="action">Делегат, выполняющий операцию и возвращающий OperationResult.</param>
       /// <returns>OperationResult с результатом выполнения делегата или с описанием ошибки. При нарушении внешнего ключа
       /// возвращается сообщение о несуществующем владельце или типе счёта; в остальных случаях — сообщение об ошибке
       /// доступа к базе данных с текстом базового исключения.</returns>
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