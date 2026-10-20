using Dapper;
using Lab1.Model;
using Microsoft.Data.SqlClient;

namespace Lab1.DataAccessLayer
{
    /// <summary>
    /// Репозиторий, реализующий CRUD-операции с использованием Dapper для объектов, соответствующих записям таблицы
    /// BankAccount.
    /// </summary>
   
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Строка подключения к базе данных, полученная из DbConfig.
        /// </summary>
    
        private readonly string _connectionString = DbConfig.ConnectionString;
        /// <summary>
        /// Создаёт новый SqlConnection, используя строку подключения _connectionString.
        /// </summary>
        /// <remarks>Вызывающий обязан открыть и освободить соединение (например, через using).</remarks>
        /// <returns>Экземпляр SqlConnection, инициализированный указанной строкой подключения; соединение не открыто.</returns>
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
        /// <summary>
        /// Возвращает все записи таблицы BankAccount в виде последовательности объектов типа T.
        /// </summary>
        /// <remarks>Сопоставление колонок с свойствами типа T выполняется по именам (используются алиасы
        /// колонок, например id_bankAccount AS Id). Подключение создаётся через CreateConnection и закрывается
        /// автоматически. Запрос выполняется с использованием Dapper.</remarks>
        /// <returns>IEnumerable<T> с объектами, сопоставленными с результатом SQL-запроса; результат материализуется в List<T>.</returns>
        public IEnumerable<T> ReadAll()
        {
            string sql = @"
                SELECT id_bankAccount AS Id, number, id_type, id_owner,
                       date_create, balance, isActiv, isDeleted
                FROM BankAccount";

            using var db = CreateConnection();
            return db.Query<T>(sql).ToList();
        }
            /// <summary>
            /// Возвращает запись таблицы BankAccount по её идентификатору.
            /// </summary>
            /// <param name="id">Идентификатор записи.</param>
            /// <returns>Объект типа T, соответствующий записи; или null, если запись не найдена.</returns>
        public T? ReadById(int id)
        {
            string sql = @"
                SELECT id_bankAccount AS Id, number, id_type, id_owner,
                       date_create, balance, isActiv, isDeleted
                FROM BankAccount
                WHERE id_bankAccount = @Id";

            using var db = CreateConnection();
            return db.QueryFirstOrDefault<T>(sql, new { Id = id });
        }
        /// <summary>
        /// Вставляет запись банковского счёта в таблицу BankAccount и устанавливает идентификатор вставленной сущности.
        /// </summary>
        /// <remarks>Выполняет INSERT с оператором OUTPUT INSERTED.id_bankAccount и использует
        /// подключение, создаваемое CreateConnection(), для получения идентификатора через
        /// ExecuteScalar<int>.</remarks>
        /// <param name="entity">Сущность банковского счёта для вставки; после выполнения Id устанавливается значением, возвращённым базой
        /// данных.</param>
        public void Add(T entity)
        {
            string sql = @"
                INSERT INTO BankAccount (number, id_type, id_owner, date_create, balance, isActiv, isDeleted)
                OUTPUT INSERTED.id_bankAccount
                VALUES (@number, @id_type, @id_owner, @date_create, @balance, @isActiv, @isDeleted)";

            using var db = CreateConnection();
            entity.Id = db.ExecuteScalar<int>(sql, entity);
        }
        /// <summary>
        /// Обновляет запись банковского счёта в таблице BankAccount.
        /// </summary>
        /// <param name="entity">Сущность банковского счёта для обновления.</param>
        public void Update(T entity)
        {
            string sql = @"
                UPDATE BankAccount
                SET number = @number, id_type = @id_type, id_owner = @id_owner,
                    date_create = @date_create, balance = @balance,
                    isActiv = @isActiv, isDeleted = @isDeleted
                WHERE id_bankAccount = @Id";

            using var db = CreateConnection();
            db.Execute(sql, entity);
        }
        /// <summary>
        /// Удаляет запись в таблице BankAccount по указанному идентификатору.
        /// </summary>
        /// <remarks>Выполняет параметризованный SQL DELETE через подключение, создаваемое
        /// CreateConnection().</remarks>
        /// <param name="id">Идентификатор удаляемой записи счёта.</param>
        public void Delete(int id)
        {
            string sql = "DELETE FROM BankAccount WHERE id_bankAccount = @Id";

            using var db = CreateConnection();
            db.Execute(sql, new { Id = id });
        }
    }
}