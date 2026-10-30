using Microsoft.Data.SqlClient;

namespace Lab1.WinForms
{
    internal static class MainFormHelpers
    {
        /// <summary>
        /// Выполняет действие и обрабатывает ошибки SQL Server.
        /// </summary>
        /// <param name="action">Действие, которое необходимо выполнить.</param>
        /// <returns>
        /// <see langword="true"/>, если действие выполнено успешно;
        /// <see langword="false"/>, если при выполнении возникло исключение SQL Server.
        /// </returns>
        private static bool Run(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (SqlException ex) when (ex.Number == 547)   
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