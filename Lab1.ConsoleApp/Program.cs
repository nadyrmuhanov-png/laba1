using System;
using System.Collections.Generic;
using System.Linq;
using Lab1.Model;
using Lab1.Model.ModelsDB;
using Lab1.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

class ConsoleApp
{
    // Контекст EF Core и репозиторий для банковских счетов
    public DBContext db = new DBContext();
    private readonly IRepository<BankAccount> _repository;

    // Выбранный в данный момент счет
    private BankAccount? focusBankAccount = null;

    public ConsoleApp()
    {
        // Инициализируем репозиторий через созданный контекст
        _repository = new EntityRepository<BankAccount>(db);
    }

    /// 
    /// Точка входа в консольное приложение.
    /// 
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

    /// 
    /// Очищает консоль и выводит шапку с информацией о текущем счете.
    /// 
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

    /// 
    /// Безопасный ввод целочисленного значения.
    /// 
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

    /// 
    /// Отображает главное меню и обрабатывает выбор пользователя через CRUD-операции IRepository.
    /// 
    public void ConsoleMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Выбрать счет");
            Console.WriteLine("2. Найти счет по номеру");
            Console.WriteLine("3. Добавить новый счет");
            Console.WriteLine("4. Изменить ID владельца счета");
            Console.WriteLine("5. Заморозить / Разморозить счет");
            Console.WriteLine("6. Удалить счет (Soft Delete / Hard Delete)");
            Console.WriteLine("7. Восстановить удаленный счет");
            Console.WriteLine("8. Показать только активные счета");
            Console.WriteLine("9. Перевод между счетами");
            Console.WriteLine("0. Выход из программы");
            Console.WriteLine(new string('-', 50));

            switch (GetNumber())
            {
                case 1:
                    var allAccounts = _repository.ReadAll().Where(a => !a.isDeleted).ToList();

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
                    Console.Write("Введите 20-значный номер счета для поиска: ");
                    string? searchNumber = Console.ReadLine();

                    var foundAccount = _repository.ReadAll()
                        .FirstOrDefault(a => a.number.Trim() == searchNumber?.Trim() && !a.isDeleted);

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

                    Console.Write("Введите номер счета (до 20 символов): ");
                    string newNum = Console.ReadLine() ?? string.Empty;

                    Console.Write("Введите ID владельца (Account ID): ");
                    int.TryParse(Console.ReadLine(), out int ownerId);

                    Console.Write("Введите ID типа счета (Type_BankAccount ID): ");
                    int.TryParse(Console.ReadLine(), out int typeId);

                    Console.Write("Введите начальный баланс: ");
                    decimal.TryParse(Console.ReadLine(), out decimal initialBalance);

                    var newAccount = new BankAccount
                    {
                        number = newNum,
                        id_owner = ownerId,
                        id_type = typeId > 0 ? typeId : 1,
                        balance = initialBalance,
                        date_create = DateTime.Today,
                        isActiv = true,
                        isDeleted = false
                    };

                    _repository.Add(newAccount);
                    focusBankAccount = newAccount;
                    ClearConsole("Новый счет успешно добавлен в базу данных.");
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
                        focusBankAccount.id_owner = newOwnerId;
                        _repository.Update(focusBankAccount);
                        ClearConsole("Владелец счета успешно обновлен в БД.");
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

                    focusBankAccount.isActiv = !focusBankAccount.isActiv;
                    _repository.Update(focusBankAccount);

                    string statusMsg = focusBankAccount.isActiv ? "разморожен и активен" : "заморожен";
                    ClearConsole($"Счет {focusBankAccount.number} успешно {statusMsg}.");
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
                        focusBankAccount.isDeleted = true;
                        focusBankAccount.isActiv = false;
                        _repository.Update(focusBankAccount);

                        focusBankAccount = null;
                        ClearConsole("Счет помечен как удаленный (Soft Delete).");
                    }
                    else
                    {
                        ClearConsole("Удаление отменено.");
                    }
                    break;

                case 7:
                    var deletedList = _repository.ReadAll().Where(a => a.isDeleted).ToList();

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
                            var toRestore = deletedList[restoreIdx - 1];
                            toRestore.isDeleted = false;
                            toRestore.isActiv = true;
                            _repository.Update(toRestore);

                            ClearConsole($"Счет {toRestore.number} успешно восстановлен.");
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
                    var activeList = _repository.ReadAll().Where(a => a.isActiv && !a.isDeleted).ToList();

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

                    if (!focusBankAccount.isActiv)
                    {
                        ClearConsole("Выбранный счет заморожен. Переводы невозможны.");
                        break;
                    }

                    Console.Write("Введите номер счета получателя: ");
                    string? targetNum = Console.ReadLine();

                    var targetAccount = _repository.ReadAll()
                        .FirstOrDefault(a => a.number.Trim() == targetNum?.Trim() && !a.isDeleted);

                    if (targetAccount == null)
                    {
                        ClearConsole("Счет получателя не найден.");
                        break;
                    }

                    if (!targetAccount.isActiv)
                    {
                        ClearConsole("Счет получателя заморожен.");
                        break;
                    }

                    Console.Write($"Введите сумму перевода (доступно {focusBankAccount.balance:C2}): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount) && transferAmount > 0)
                    {
                        if (focusBankAccount.balance >= transferAmount)
                        {
                            focusBankAccount.balance -= transferAmount;
                            targetAccount.balance += transferAmount;

                            _repository.Update(focusBankAccount);
                            _repository.Update(targetAccount);

                            ClearConsole($"Перевод на сумму {transferAmount:C2} успешно выполнен!");
                        }
                        else
                        {
                            ClearConsole("Недостаточно средств на счете!");
                        }
                    }
                    else
                    {
                        ClearConsole("Некорректная сумма перевода.");
                    }
                    break;

                case 0:
                    Console.WriteLine("Выход из программы...");
                    return;

                default:
                    ClearConsole("Неверный пункт меню.");
                    break;
            }
        }
    }
}