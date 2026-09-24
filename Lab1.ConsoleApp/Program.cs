using laba1.Models;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

class ConsoleApp 
{
    private BankAccount focusBankAccount = new BankAccount();
    private Logic logic = new Logic();
    /// <summary>
    /// Точка входа в консольное приложение. Выводит приветственное сообщение и запускает главное меню.
    /// </summary>
    public static void Main()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("==================================================");
        Console.WriteLine("    СИСТЕМА УПРАВЛЕНИЯ БАНКОВСКИМИ СЧЕТАМИ ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine("Добро пожаловать! Выберите действие из списка ниже:\n");

        ConsoleApp app = new ConsoleApp();

        app.ConsoleMenu();

    }

    /// <summary>
    /// Очищает консоль и выводит приветственное сообщение, а также информацию о выбранном счёте, если он есть.
    /// </summary>
    /// <param name="messageBeforeClear">Сообщение, которое нужно вывести перед очисткой консоли</param>
    private void ClearConsole(string? messageBeforeClear) 
    {
        Console.Clear();

        if (messageBeforeClear != "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(messageBeforeClear);
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("    СИСТЕМА УПРАВЛЕНИЯ БАНКОВСКИМИ СЧЕТАМИ ");
        Console.WriteLine("==================================================");

        if (focusBankAccount.AccountNumber != null) 
        {
            Console.Write("ВЫБРАННЫЙ СЧЕТ: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Номер счета: {focusBankAccount.AccountNumber}, Владелец: {focusBankAccount.AccountOwner}, Баланс: {focusBankAccount.Balance}, Статус: {(focusBankAccount.IsActive ? "Активен" : "Заморожен")}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ВЫБРАННЫЙ СЧЕТ: Не выбран");
            Console.ResetColor();
        }

        ConsoleMenu();
    }
    /// <summary>
    /// Запрашивает у пользователя ввод номера действия и возвращает его в виде целого числа. Если ввод некорректен, выводит сообщение об ошибке.
    /// </summary>
    /// <returns></returns>
    private int GetNumber() 
    {
        Console.Write("Введите номер действия: ");

        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");

        }

        Console.WriteLine();


        return number;

    }
    /// <summary>
    /// Отображает главное меню консольного приложения и обрабатывает выбор пользователя.   
    /// </summary>
    public void ConsoleMenu() 
    {
        while (true) 
        {
           

            Console.WriteLine("1. Выбрать счет");
            Console.WriteLine("2. Найти счет по номеру");
            Console.WriteLine("3. Добавить новый счет");
            Console.WriteLine("4. Изменить владельца счета");
            Console.WriteLine("5. Заморозить / Разморозить счет");
            Console.WriteLine("6. Удалить счет");
            Console.WriteLine("7. Восстановить счет");
            Console.WriteLine("8. Показать только активные счета");
            Console.WriteLine("9. Перевод");
            Console.WriteLine("0. Выход из программы");
            Console.WriteLine(new string('-', 50));

            

            switch (GetNumber())
            {

                case 1:

                    List<BankAccount> bankAccounts = new List<BankAccount>();

                    if (logic.Accounts.Count > 0)
                    {
                        bankAccounts = logic.Accounts;

                        int counter = 1;
                        foreach (BankAccount account in bankAccounts)
                        {
                            Console.WriteLine($"{counter}. Номер счета: {account.AccountNumber}, Владелец: {account.AccountOwner}");
                            counter++;
                        }
                        

                        int number = GetNumber();

                        focusBankAccount = bankAccounts[number - 1];

                        ClearConsole("Счет выбран.");
                    }
                    else
                    {
                        ClearConsole("Список счетов пуст. Пожалуйста, добавьте новый счет.");
                        Console.WriteLine();
                    }

                    break;
                case 2:

                    while (true)
                    {
                        Console.Write("Введите номер счета для поиска: ");
                        string accountNumber = Console.ReadLine();

                        if (logic.Accounts.Count > 0)
                        {
                            bankAccounts = logic.Accounts;

                            BankAccount? foundAccount = bankAccounts.Find(account => account.AccountNumber == accountNumber);
                            if (foundAccount != null)
                            {
                                Console.WriteLine($"Найден счет: Номер: {foundAccount.AccountNumber}, Владелец: {foundAccount.AccountOwner}");
                            }
                            else
                            {
                                Console.WriteLine("Счет не найден.");
                            }

                            Console.WriteLine("1. Найти другой счет");
                            Console.WriteLine("2. Вернуться в главное меню");

                            int choice = GetNumber();

                            switch (choice) 
                            {
                                case 1:
                                    continue;
                                case 2:
                                    ClearConsole(null);
                                    break;


                            }
                        }
                        else
                        {
                            ClearConsole("Список счетов пуст. Пожалуйста, добавьте новый счет.");
                            Console.WriteLine();
                        }

                    }
                case 3:
                    Console.WriteLine("Добавление нового счета:\n");
                    Console.Write("Введите номер счета: ");
                    string newAccountNumber = Console.ReadLine();

                    Console.Write("Введите имя владельца счета: ");
                    string newAccountOwner = Console.ReadLine();

                    BankAccount newAccount = new BankAccount
                    {
                        AccountNumber = newAccountNumber,
                        AccountOwner = newAccountOwner,
                        Balance = 0,
                        IsActive = true
                    };

                    logic.AddAccount(newAccount);

                    
                    ClearConsole("Новый счет успешно добавлен.");
                    

                    break;
                case 4:
                    Console.WriteLine("Изменение владельца счета:\n");

                    Console.WriteLine("Введите новый владелец счета: ");
                    string newOwner = Console.ReadLine();

                    if (logic.EditAccountOwner(focusBankAccount.AccountNumber, newOwner)) 
                    {
                        
                        ClearConsole("Владелец счета успешно изменен.");
                       
                    }
                    else
                    {
                       
                        ClearConsole("Ошибка при изменении владельца счета.");
                        
                    }

                    break;
                case 5:
                    Console.WriteLine("Заморозка / Разморозка счета:\n");
                    if (focusBankAccount.AccountNumber != null)
                    {
                        if (focusBankAccount.IsActive)
                        {
                            logic.FreezeAccountNumber(focusBankAccount.AccountNumber);

                            ClearConsole("Счет успешно заморожен.");
                        }
                        else
                        {
                            logic.UnfreezeAccountNumber(focusBankAccount.AccountNumber);

                            ClearConsole("Счет успешно разморожен.");
                        }

                    }
                    else
                        ClearConsole("Аккаунт не выбран, выберите аккаунт.");
                    

                    break;
                case 6:
                     Console.WriteLine("Удаление счета:\n");
                     Console.WriteLine("Вы уверены, что хотите удалить счет? (y/n)");
                     char confirmation = Console.ReadKey().KeyChar;

                    if (confirmation == 'y' || confirmation == 'Y')
                    {
                        if(focusBankAccount.RemouveAccount())
                        {
                            focusBankAccount = null;
                            ClearConsole("Счет успешно удален.");
                            
                        }
                        else
                        {
                            
                            ClearConsole("Ошибка при удалении счета. Проверьте баланс счета.");
                        }
                    }
                    else 
                    {
                        
                        ClearConsole("Удаление счета отменено.");

                    }
                    break;
                case 7:
                    Console.WriteLine("Восстановление счета:\n");

                    List<BankAccount> deletedAccounts = logic.GetDeletedAccount();

                    int counterDeleted = 1;

                    if (deletedAccounts.Count > 0)
                    {
                        Console.WriteLine("Доступные удаленные счета:");
                        foreach (BankAccount account in deletedAccounts)
                        {
                            Console.WriteLine($"{counterDeleted}. Номер счета: {account.AccountNumber}, Владелец: {account.AccountOwner}");
                            counterDeleted++;
                        }

                        Console.WriteLine("Введите номер счета для восстановления: ");
                        int restoreChoice = GetNumber();

                        if (restoreChoice >= 1 && restoreChoice <= deletedAccounts.Count)
                        {
                            if (deletedAccounts[restoreChoice - 1].RestoreAccount())
                            {
                                
                                ClearConsole("Счет успешно восстановлен.");
                            }
                            else
                            {
                                
                                ClearConsole("Ошибка при восстановлении счета.");
                            }
                        }
                        else
                        {
                            
                            ClearConsole("Некорректный выбор. Пожалуйста, выберите счет из списка.");
                        }

                    }
                    else 
                    {
                        
                        ClearConsole("Удаленные счета отсутствуют.");
                    }

                   
                    break;
                case 8:
                    Console.WriteLine("Список активных счетов:\n");
                    List<BankAccount> activeAccounts = logic.GetActiveAccount();
                    if (activeAccounts.Count > 0)
                    {
                        foreach (BankAccount account in activeAccounts)
                        {
                            Console.WriteLine($"Номер счета: {account.AccountNumber}, Владелец: {account.AccountOwner}, Баланс: {account.Balance}");
                        }
                        Console.WriteLine("\nНажмите любую кнопку что-бы продолжить...\n");
                        Console.ReadLine();
                        ClearConsole(null);
                    }
                    else
                    {
                        ClearConsole("Активные счета отсутствуют.");
                    }
                    break;
                case 9:
                    while (true) 
                    {
                        if (focusBankAccount.AccountNumber != null)
                        {
                            Console.WriteLine($"Отправка перевода, от имени {focusBankAccount.AccountOwner}.");
                            Console.Write("Введите номер счета получателя: ");
                            string targetNumber = Console.ReadLine();

                            if (logic.GetAccount(targetNumber) != null)
                            {
                                while (true) 
                                {
                                    Console.WriteLine("Введите сумму: ");
                                    if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                    {
                                        if (logic.Transfer(focusBankAccount, logic.GetAccount(targetNumber), amount))
                                        {
                                            ClearConsole("Перевод прошел успешно.");
                                        }
                                        else ClearConsole("Ошибка при отправке перевода. Проверьте данные.");


                                }
                                    else
                                    {
                                        Console.WriteLine("Введите подходящую сумму.");
                                        continue;
                                    }
                                }
                               

                            }
                            else 
                            {
                                Console.WriteLine("Аккаунт не найден, повторите попытку.");
                                continue;
                            }



                        }
                        else
                        {
                            ClearConsole("Аккаунт не выбран.");
                        }
                    }
                   
                case 0:
                    Console.WriteLine("Выход из программы...");
                    return;
                default:
                    ClearConsole("Некорректный выбор. Пожалуйста, выберите действие из списка.");
                    break;
            }

        }
    
    }
}