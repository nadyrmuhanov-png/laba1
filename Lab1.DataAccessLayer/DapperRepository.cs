using Dapper;
using Lab1.Model;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Lab1.DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория для работы с доменными объектами типа T с использованием Dapper.
    /// </summary>
    /// <typeparam name="T">
    /// Тип доменного объекта, с которым работает репозиторий.
    /// </typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Массив свойств типа T, которые будут использоваться для операций с базой данных.
        /// </summary>
        private static readonly PropertyInfo[] Props = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite
                        && IsSimple(p.PropertyType)
                        && p.GetCustomAttribute<NotMappedAttribute>() == null)
            .ToArray();
        /// <summary>
        /// Массив свойств типа T, которые будут использоваться для операций с базой данных, за исключением свойства Id.
        /// </summary>
        private static readonly PropertyInfo[] DataProps =
            Props.Where(p => p.Name != nameof(IDomainObject.Id)).ToArray();
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        /// <param name="p">Свойство типа T.</param>
        /// <returns>Имя столбца в базе данных.</returns>

        private static string ColumnOf(PropertyInfo p) =>
            p.GetCustomAttribute<ColumnAttribute>()?.Name ?? p.Name;
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        /// <param name="p">Свойство типа T.</param>
        /// <returns>Имя столбца в базе данных.</returns>

        private static readonly string IdColumn =
            ColumnOf(typeof(T).GetProperty(nameof(IDomainObject.Id))!);
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        /// <param name="type">Тип свойства.</param>
        /// <returns>Имя столбца в базе данных.</returns>
        private static bool IsSimple(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.IsPrimitive || type.IsEnum
                   || type == typeof(string) || type == typeof(decimal)
                   || type == typeof(DateTime) || type == typeof(DateOnly)
                   || type == typeof(DateTimeOffset) || type == typeof(Guid);
        }
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        private readonly string _connectionString;
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        private readonly string _tableName;
        /// <summary>
        /// Возвращает имя столбца в базе данных, соответствующее свойству типа T.
        /// </summary>
        private readonly string _selectList;

        /// <param name="connectionString">Строка подключения к SQL Server.</param>
        /// <param name="tableName">Имя таблицы (для BankAccount — "BankAccount").</param>
        public DapperRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            // id_bankAccount AS Id, number AS number, ...
            _selectList = string.Join(", ", Props.Select(p => $"{ColumnOf(p)} AS {p.Name}"));
        }
        /// <summary>
        /// Создает и возвращает новое подключение к базе данных SQL Server.
        /// </summary>
        /// <returns>
        /// Новое подключение к базе данных SQL Server.
        /// </returns>
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);
        /// <summary>
        /// Возвращает все объекты типа T из базы данных./// </summary>
        /// <returns>
        /// Список всех объектов типа T.
        /// </returns>
        public IEnumerable<T> ReadAll()
        {
            using var db = CreateConnection();
            return db.Query<T>($"SELECT {_selectList} FROM {_tableName}").ToList();
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
                $"SELECT {_selectList} FROM {_tableName} WHERE {IdColumn} = @Id",
                new { Id = id });
        }
        /// <summary>
        /// Добавляет новый объект типа T в базу данных и возвращает его с обновленным идентификатором.
        /// </summary>
        /// <param name="entity">Объект типа T для добавления.</param>
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
        /// Обновляет запись BankAccount в базе данных по идентификатору, устанавливая поля из переданного объекта.
        /// </summary>
        /// <remarks>Выполняет SQL UPDATE через Dapper; соединение создаётся через CreateConnection и
        /// автоматически закрывается. Параметры SQL сопоставляются с именами свойств объекта.</remarks>
        /// <param name="entity">Объект с данными для обновления; должен содержать свойства, соответствующие параметрам SQL (включая Id).</param>
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
        /// Удаляет запись банковского счёта с указанным идентификатором.
        /// </summary>
        /// <remarks>Выполняет непосредственное удаление через SQL; не проверяет существование записи и не
        /// использует транзакцию.</remarks>
        /// <param name="id">Идентификатор удаляемой записи банковского счёта.</param>
        public void Delete(int id)
        {
            string sql = "DELETE FROM BankAccount WHERE id_bankAccount = @Id";

            using var db = CreateConnection();
            db.Execute(sql, new { Id = id });
        }
    }
}