using Lab1.BusinessLogic;
using Lab1.DataAccessLayer;
using Lab1.Model;
using Lab1.Model.ModelsDB;

class ConsoleApp
{
    /// <summary>
    /// Экземпляр Logic, настроенный с EntityRepository<BankAccount> и DBContext, обеспечивает бизнес-операции над
    /// банковскими счетами.
    /// </summary>
    
    private readonly Logic _logic = new Logic(new EntityRepository<BankAccount>(new DBContext()));

    /// <summary>
    /// Фокусируемый банковский счёт, допускающий значение null.
    /// </summary>
    /// <remarks>Хранит временно выбранный счёт (например, для UI или операций); по умолчанию null.</remarks>
    private BankAccount? focusBankAccount = null;

    /// <summary>Точка входа в консольное приложение.</summary>
    public static void Main()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("==================================================");
        Console.WriteLine("    СИСТЕМА УПРАВЛЕНИЯ БАНКОВСКИМИ СЧЕТАМИ        ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine("Добро пожаловать! Выберите действие из списка ниже:\n");

        ConsoleApp app = new ConsoleApp();
        app.ConsoleMenu();
    }

    /// <summary>
    /// Очищает консоль и отображает заголовок приложения, необязательное сообщение и сведения о выбранном банковском
    /// счёте.
    /// </summary>
    
    /// <param name="messageBeforeClear">Необязательное сообщение, выводимое после очистки консоли; если null или пусто, пропускается. Выводится жёлтым
    /// цветом.</param>
    private void ClearConsole(string? messageBeforeClear)
    {
        Console.Clear();

        if (!string.IsNullOrEmpty(messageBeforeClear))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(messageBeforeClear);
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("    СИСТЕМА УПРАВЛЕНИЯ БАНКОВСКИМИ СЧЕТАМИ        ");
        Console.WriteLine("==================================================");

        if (focusBankAccount != null && !string.IsNullOrEmpty(focusBankAccount.number))
        {
            Console.Write("ВЫБРАННЫЙ СЧЕТ: ");
            Console.ForegroundColor = ConsoleColor.Green;

            string owner = focusBankAccount.Account?.full_name ?? $"ID Владельца: {focusBankAccount.id_owner}";
            string status = focusBankAccount.isActiv ? "Активен" : "Заморожен";

            Console.WriteLine($"Номер: {focusBankAccount.number}, Владелец: {owner}, Баланс: {focusBankAccount.balance:C2}, Статус: {status}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ВЫБРАННЫЙ СЧЕТ: Не выбран");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Запрашивает у пользователя номер действия через консоль и возвращает введённое целое число.
    /// </summary>
    /// <returns>Целое число, введённое пользователем.</returns>
    private int GetNumber()
    {
        Console.Write("Введите номер действия: ");
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.Write("Некорректный ввод. Пожалуйста, введите число: ");
        }
        Console.WriteLine();
        return number;
    }

    /// <summary>
    /// Обновляет ссылку focusBankAccount актуальным экземпляром, полученным по идентификатору через _logic.GetById,
    /// если она не равна null.
    /// </summary>
 
    private void RefreshFocus()
    {
        if (focusBankAccount != null)
            focusBankAccount = _logic.GetById(focusBankAccount.Id);
    }

    /// <summary>
    /// Отображает интерактивное консольное меню для управления банковскими счетами и обрабатывает ввод пользователя.
    /// </summary>
    
    public void ConsoleMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Выбрать счет");
            Console.WriteLine("2. Найти счет по номеру");
            Console.WriteLine("3. Добавить новый счет");
            Console.WriteLine("4. Изменить ID владельца счета");
            Console.WriteLine("5. Заморозить / Разморозить счет");
            Console.WriteLine("6. Удалить счет (Soft Delete)");
            Console.WriteLine("7. Восстановить удаленный счет");
            Console.WriteLine("8. Показать только активные счета");
            Console.WriteLine("9. Перевод между счетами");
            Console.WriteLine("0. Выход из программы");
            Console.WriteLine(new string('-', 50));

            int action = GetNumber();
            if (action == 0)
            {
                Console.WriteLine("Выход из программы...");
                return;
            }

            try
            {
                switch (action)
                {
                    case 1:
                        var allAccounts = _logic.GetAccounts(deleted: false);

                        if (allAccounts.Count > 0)
                        {
                            Console.WriteLine("Список доступных счетов:");
                            for (int i = 0; i < allAccounts.Count; i++)
                            {
                                var acc = allAccounts[i];
                                string owner = acc.Account?.full_name ?? $"ID: {acc.id_owner}";
                                Console.WriteLine($"{i + 1}. Номер: {acc.number} | Баланс: {acc.balance:C2} | Владелец: {owner}");
                            }

                            Console.Write("\nВыберите порядковый номер счета: ");
                            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= allAccounts.Count)
                            {
                                focusBankAccount = allAccounts[index - 1];
                                ClearConsole("Счет успешно выбран.");
                            }
                            else
                            {
                                ClearConsole("Неверный номер в списке.");
                            }
                        }
                        else
                        {
                            ClearConsole("Список счетов пуст. Добавьте новый счет через меню.");
                        }
                        break;

                    case 2:
                        Console.Write("Введите номер счета для поиска: ");
                        string? searchNumber = Console.ReadLine();

                        var foundAccount = _logic.FindByNumber(searchNumber);

                        if (foundAccount != null)
                        {
                            Console.WriteLine($"\nНайден счет! ID: {foundAccount.Id}, Номер: {foundAccount.number}, Баланс: {foundAccount.balance:C2}");
                            Console.WriteLine("Сделать его активным для работы? (y/n)");
                            if (Console.ReadKey().KeyChar is 'y' or 'Y')
                            {
                                focusBankAccount = foundAccount;
                                ClearConsole("Счет выбран как текущий.");
                            }
                            else
                            {
                                ClearConsole(null);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nСчет с таким номером не найден.");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            ClearConsole(null);
                        }
                        break;

                    case 3:
                        Console.WriteLine("Добавление нового счета:\n");

                        Console.Write("Введите номер счета (до 20 цифр): ");
                        string newNum = Console.ReadLine() ?? string.Empty;

                        Console.Write("Введите ID владельца (Account ID): ");
                        int.TryParse(Console.ReadLine(), out int ownerId);

                        Console.Write("Введите ID типа счета (Type_BankAccount ID, 1 по умолчанию): ");
                        int.TryParse(Console.ReadLine(), out int typeId);

                        Console.Write("Введите начальный баланс: ");
                        decimal.TryParse(Console.ReadLine(), out decimal initialBalance);

                        var addResult = _logic.AddAccount(newNum, ownerId, typeId, initialBalance);
                        if (addResult.Success)
                            focusBankAccount = _logic.FindByNumber(newNum);
                        ClearConsole(addResult.Message);
                        break;

                    case 4:
                        if (focusBankAccount == null)
                        {
                            ClearConsole("Счет не выбран. Сначала выберите счет в пункте 1.");
                            break;
                        }

                        Console.Write($"Текущий ID владельца: {focusBankAccount.id_owner}. Введите новый ID владельца: ");
                        if (int.TryParse(Console.ReadLine(), out int newOwnerId))
                        {
                            var ownerResult = _logic.ChangeOwner(focusBankAccount.Id, newOwnerId);
                            RefreshFocus();
                            ClearConsole(ownerResult.Message);
                        }
                        else
                        {
                            ClearConsole("Некорректный ID владельца.");
                        }
                        break;

                    case 5:
                        if (focusBankAccount == null)
                        {
                            ClearConsole("Счет не выбран. Сначала выберите счет.");
                            break;
                        }

                        var freezeResult = _logic.ToggleFreeze(focusBankAccount.Id);
                        RefreshFocus();
                        ClearConsole(freezeResult.Message);
                        break;

                    case 6:
                        if (focusBankAccount == null)
                        {
                            ClearConsole("Счет не выбран.");
                            break;
                        }

                        Console.WriteLine($"Вы уверены, что хотите удалить счет {focusBankAccount.number}? (y/n)");
                        char confirm = Console.ReadKey().KeyChar;

                        if (confirm is 'y' or 'Y')
                        {
                            var deleteResult = _logic.DeleteAccount(focusBankAccount.Id);
                            if (deleteResult.Success)
                                focusBankAccount = null;
                            else
                                RefreshFocus();
                            ClearConsole(deleteResult.Message);
                        }
                        else
                        {
                            ClearConsole("Удаление отменено.");
                        }
                        break;

                    case 7:
                        var deletedList = _logic.GetAccounts(deleted: true);

                        if (deletedList.Count > 0)
                        {
                            Console.WriteLine("Список удаленных счетов:");
                            for (int i = 0; i < deletedList.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. Номер: {deletedList[i].number} (Баланс: {deletedList[i].balance:C2})");
                            }

                            Console.Write("Введите порядковый номер для восстановления: ");
                            if (int.TryParse(Console.ReadLine(), out int restoreIdx) && restoreIdx >= 1 && restoreIdx <= deletedList.Count)
                            {
                                var restoreResult = _logic.RestoreAccount(deletedList[restoreIdx - 1].Id);
                                ClearConsole(restoreResult.Message);
                            }
                            else
                            {
                                ClearConsole("Неверный выбор.");
                            }
                        }
                        else
                        {
                            ClearConsole("Удаленные счета отсутствуют.");
                        }
                        break;

                    case 8:
                        var activeList = _logic.GetActiveAccounts();

                        if (activeList.Count > 0)
                        {
                            Console.WriteLine("--- АКТИВНЫЕ СЧЕТА В БАЗЕ ДАННЫХ ---");
                            foreach (var acc in activeList)
                            {
                                string owner = acc.Account?.full_name ?? $"ID: {acc.id_owner}";
                                Console.WriteLine($"Номер: {acc.number} | Баланс: {acc.balance:C2} | Владелец: {owner}");
                            }
                            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                            Console.ReadKey();
                            ClearConsole(null);
                        }
                        else
                        {
                            ClearConsole("Активных счетов не найдено.");
                        }
                        break;

                    case 9:
                        if (focusBankAccount == null)
                        {
                            ClearConsole("Счет списания не выбран! Сначала выберите счет в пункте 1.");
                            break;
                        }

                        Console.Write("Введите номер счета получателя: ");
                        string? targetNum = Console.ReadLine();

                        Console.Write($"Введите сумму перевода (доступно {focusBankAccount.balance:C2}): ");
                        decimal.TryParse(Console.ReadLine(), out decimal transferAmount);

                        var transferResult = _logic.Transfer(focusBankAccount.Id, targetNum, transferAmount);
                        RefreshFocus();
                        ClearConsole(transferResult.Message);
                        break;

                    default:
                        ClearConsole("Неверный пункт меню.");
                        break;
                }
            }
            catch (Exception ex)
            {
                
                ClearConsole("Ошибка: " + ex.GetBaseException().Message);
            }
        }
    }
}