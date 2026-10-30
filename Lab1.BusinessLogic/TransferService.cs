using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;

namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Отвечает за выполнение переводов между банковскими счетами
    /// и проверку бизнес-правил, связанных с переводами.
    /// </summary>
    public class TransferService
    {
        private readonly IRepository<BankAccount> repository;

        public TransferService(IRepository<BankAccount> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Выполняет перевод средств между счетами.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="targetNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public OperationResult Transfer(int senderId, string? targetNumber, decimal amount)
        {
            if (amount <= 0)
                return OperationResult.Fail("Сумма перевода должна быть положительной.");

            return Execute(() =>
            {
                var sender = repository.ReadById(senderId);
                if (sender == null || sender.isDeleted)
                    return OperationResult.Fail("Счёт-отправитель не найден.");

                var target = repository.ReadAll().FirstOrDefault(a =>
                    a.number.Trim() == (targetNumber ?? string.Empty).Trim() && !a.isDeleted);

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
        /// Выполняет указанное действие и обрабатывает возможные исключения.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
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
