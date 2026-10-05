
using Dapper;
using laba1.Models;
using Microsoft.Data.SqlClient;

namespace Lab1.DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория с использованием Dapper для работы с базой данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperRepository<T> : IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DapperRepository с указанной строкой подключения и именем таблицы.
        /// </summary>
        private readonly string _connectionString;
        /// <summary>
        /// Получает имя таблицы.
        /// </summary>
        private readonly string _tableName;
        /// <summary>
        /// Инициализирует новый экземпляр класса DapperRepository с указанной строкой подключения и именем таблицы.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        /// <param name="tableName">Имя таблицы.</param>
        public DapperRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }
        /// <summary>
        /// Создаёт и возвращает новое подключение к базе данных SQL Server.
        /// </summary>
        /// <returns>
        /// Новое подключение к базе данных SQL Server.
        /// </returns>
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
        /// <summary>
        /// Возвращает все объекты типа T из базы данных.
        /// </summary>
        /// <returns>
        /// Список всех объектов типа T.
        /// </returns>
        public IEnumerable<T> ReadAll()
        {
            using var db = CreateConnection();
            return db.Query<T>($"SELECT * FROM {_tableName}");
        }
        /// <summary>
        /// Возвращает объект типа T с указанным идентификатором из базы данных.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект типа T или null, если объект не найден.</returns>
        public T? ReadById(int id)
        {
            using var db = CreateConnection();
            return db.QueryFirstOrDefault<T>(
                $"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }
        /// <summary>
        /// Добавляет новый объект типа T в базу данных.
        /// </summary>
        /// <param name="entity">Объект типа T для добавления.</param>
        public void Add(T entity)
        {
            using var db = CreateConnection();
            db.Execute(
                $@"INSERT INTO {_tableName} (AccountNumber, AccountOwner, Balance, IsActive, IsDeleted)
                   VALUES (@AccountNumber, @AccountOwner, @Balance, @IsActive, @IsDeleted)",
                entity);
        }
        /// <summary>
        /// Обновляет существующий объект типа T в базе данных.
        /// </summary>
        /// <param name="entity">Объект типа T для обновления.</param>
        public void Update(T entity)
        {
            using var db = CreateConnection();
            db.Execute(
                $@"UPDATE {_tableName}
                   SET AccountOwner=@AccountOwner, Balance=@Balance, IsActive=@IsActive, IsDeleted=@IsDeleted
                   WHERE Id=@Id",
                entity);
        }
        /// <summary>
        /// Удаляет объект типа T с указанным идентификатором из базы данных.
        /// </summary>
        /// <param name="id">Идентификатор объекта для удаления.</param>
        public void Delete(int id)
        {
            using var db = CreateConnection();
            db.Execute($"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });
        }
    }
}