
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using Microsoft.Data.SqlClient;
using Lab1.Model;

namespace Lab1.DataAccessLayer
{
    ///<summary>
    /// Репозиторий для выполнения CRUD-операций с сущностями с помощью Dapper.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующей <see cref="IDomainObject"/>.</typeparam>
    public class DapperRepository<T> : IRepository<T>
        where T : class, IDomainObject
    {
      
        private readonly string connectionString;
        
        private readonly string tableName;
        
        private readonly List<PropertyInfo> properties;
        /// <summary>
        /// Создаёт репозиторий и определяет таблицу и свойства сущности,
        /// которые будут использоваться при работе с базой данных.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public DapperRepository(string connectionString)
        {
            this.connectionString = connectionString;

            var tableAttribute =
                typeof(T).GetCustomAttribute<TableAttribute>();

            tableName = tableAttribute?.Name ?? typeof(T).Name;

            properties = typeof(T).GetProperties()
                .Where(p => p.CanRead &&
                            p.CanWrite &&
                            p.GetIndexParameters().Length == 0 &&
                            IsSimpleType(p.PropertyType))
                .ToList();
        }

        /// <summary>
        /// Проверяем, подходит ли тип свойства для хранения в таблице.
        /// </summary>
        /// <param name="type">Тип свойства.</param>
        /// <returns>True, если тип подходит, иначе False.</returns>
        private bool IsSimpleType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsPrimitive ||
                   type.IsEnum ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(Guid) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(byte[]);
        }

        /// <summary>
        /// Получаем имя столбца из атрибута Column.
        /// </summary>
        /// <param name="property">Свойство, для которого нужно получить имя столбца.</param>
        /// <returns>Имя столбца.</returns>
        private string GetColumnName(PropertyInfo property)
        {
            var columnAttribute =
                property.GetCustomAttribute<ColumnAttribute>();

            return columnAttribute?.Name ?? property.Name;
        }

        /// <summary>
        /// Защищаем имя таблицы или столбца квадратными скобками.
        /// </summary>
        /// <param name="name">Имя таблицы или столбца.</param>
        /// <returns>Имя с квадратными скобками.</returns>
        private string SqlName(string name)
        {
            return "[" + name.Replace("]", "]]") + "]";
        }

        /// <summary>
        /// Находим свойство Id.
        /// </summary>
        /// <returns>Свойство, представляющее идентификатор.</returns>
        private PropertyInfo GetIdProperty()
        {
            var idProperty = properties
                .FirstOrDefault(p => p.Name == "Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException(
                    "У модели " + typeof(T).Name +
                    " отсутствует свойство Id.");
            }

            return idProperty;
        }

        /// <summary>
        /// Получаем список столбцов для SELECT.
        /// </summary>
        /// <returns>Строка с именами столбцов.</returns>
        private string GetSelectColumns()
        {
            return string.Join(", ",
                properties.Select(p =>
                    SqlName(GetColumnName(p)) +
                    " AS " + SqlName(p.Name)));
        }

        /// <summary>
        /// Создаёт подключение к базе данных.
        /// </summary>
        /// <returns>Новое подключение SQL Server.</returns>
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Получает все записи.
        /// </summary>
        /// <returns>Перечисление записей.</returns>
        public IEnumerable<T> ReadAll()
        {
            using (var connection = CreateConnection())
            {
                string sql =
                    "SELECT " + GetSelectColumns() +
                    " FROM " + SqlName(tableName);

                return connection.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// Ищет запись по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи.</param>
        /// <returns>Найденная сущность или <see langword="null"/>, если запись не найдена.</returns>
        public T? ReadById(int id)
        {
            using (var connection = CreateConnection())
            {
                PropertyInfo idProperty = GetIdProperty();

                string idColumn =
                    SqlName(GetColumnName(idProperty));

                string sql =
                    "SELECT " + GetSelectColumns() +
                    " FROM " + SqlName(tableName) +
                    " WHERE " + idColumn + " = @Id";

                return connection.QueryFirstOrDefault<T>(
                    sql, new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет новую запись и устанавливает сгенерированный идентификатор сущности.
        /// </summary>
        /// <param name="item">Добавляемая сущность.</param>
        public void Add(T item)
        {
            using (var connection = CreateConnection())
            {
                PropertyInfo idProperty = GetIdProperty();

                var insertProperties = properties
                    .Where(p => p.Name != "Id")
                    .ToList();

                string columnNames = string.Join(", ",
                    insertProperties.Select(p =>
                        SqlName(GetColumnName(p))));

                string parameterNames = string.Join(", ",
                    insertProperties.Select(p =>
                        "@" + p.Name));

                string idColumn =
                    SqlName(GetColumnName(idProperty));

                string sql =
                    "INSERT INTO " + SqlName(tableName) +
                    " (" + columnNames + ") " +
                    "OUTPUT INSERTED." + idColumn +
                    " VALUES (" + parameterNames + ")";

                int newId =
                    connection.ExecuteScalar<int>(sql, item);

                idProperty.SetValue(item, newId);
            }
        }

        /// <summary>
        /// Обновляет запись, соответствующую идентификатору сущности.
        /// </summary>
        /// <param name="item">Сущность с обновлёнными данными.</param>
        public void Update(T item)
        {
            using (var connection = CreateConnection())
            {
                PropertyInfo idProperty = GetIdProperty();

                var updateProperties = properties
                    .Where(p => p.Name != "Id")
                    .ToList();

                string setValues = string.Join(", ",
                    updateProperties.Select(p =>
                        SqlName(GetColumnName(p)) +
                        " = @" + p.Name));

                string idColumn =
                    SqlName(GetColumnName(idProperty));

                string sql =
                    "UPDATE " + SqlName(tableName) +
                    " SET " + setValues +
                    " WHERE " + idColumn + " = @Id";

                connection.Execute(sql, item);
            }
        }

        /// <summary>
        /// Удаляет запись по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        public void Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                PropertyInfo idProperty = GetIdProperty();

                string idColumn =
                    SqlName(GetColumnName(idProperty));

                string sql =
                    "DELETE FROM " + SqlName(tableName) +
                    " WHERE " + idColumn + " = @Id";

                connection.Execute(sql, new { Id = id });
            }
        }
    }
}
