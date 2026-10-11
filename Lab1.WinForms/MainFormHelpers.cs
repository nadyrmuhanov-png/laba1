using Microsoft.Data.SqlClient;

namespace Lab1.WinForms
{
    internal static class MainFormHelpers
    {
        private static bool Run(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (SqlException ex) when (ex.Number == 547)   // нарушение внешнего ключа
            {
                MessageBox.Show("Владельца с таким ID не существует. Допустимые ID: 1–5.");
                return false;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка базы данных: " + ex.Message);
                return false;
            }
        }
    }
}