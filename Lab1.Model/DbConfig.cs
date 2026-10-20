namespace Lab1.Model
{
    /// <summary>Единое место для строки подключения к БД (используют и EF, и Dapper).</summary>
    public static class DbConfig
    {
        public static string ConnectionString =>
            Environment.GetEnvironmentVariable("LAB1_DB")
            ?? @"Server=localhost\SQLEXPRESS;Database=dbDataBank;Trusted_Connection=True;Persist Security Info=False;Encrypt=True;TrustServerCertificate=True;";
    }
}