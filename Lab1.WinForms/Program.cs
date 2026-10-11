using Lab1.DataAccessLayer;
using Lab1.Model.ModelsDB;

namespace Lab1.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Сценарий №2 из методички: работа с БД через Dapper.
            IRepository<BankAccount> repository = new DapperRepository<BankAccount>(
    @"Server=.\SQLEXPRESS;Database=dbDataBank;Trusted_Connection=True;TrustServerCertificate=True;",
    "BankAccount");

            Application.Run(new MainForm(repository));
        }
    }
}