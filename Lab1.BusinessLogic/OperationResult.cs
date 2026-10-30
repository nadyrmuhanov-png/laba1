namespace Lab1.BusinessLogic
{
    /// <summary>
    /// Результат выполнения операции, содержащий признак успешности и сообщение.
    /// </summary>
    public record OperationResult(bool Success, string Message)
    {
        /// <summary>
        /// Создаёт успешный результат операции.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>
        /// Успешный результат операции.
        /// </returns>
        public static OperationResult Ok(string message = "") => new(true, message);
        public static OperationResult Fail(string message) => new(false, message);
    }
}
