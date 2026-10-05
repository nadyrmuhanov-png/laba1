using Lab1.Model;

namespace Lab1.DataAccessLayer
{
    /// <summary>
    /// Интерфейс репозитория для работы с доменными объектами типа T.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Возвращает объект типа T с указанным идентификатором из базы данных.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        /// <returns>Объект типа T или null, если объект не найден.</returns>
        T? ReadById(int id);
        /// <summary>
        /// Возвращает все объекты типа T из базы данных.
        /// </summary>
        /// <returns>Список всех объектов типа T.</returns>
        IEnumerable<T> ReadAll();
        /// <summary>
        /// Добавляет новый объект типа T в базу данных.
        /// </summary>
        /// <param name="entity">Объект типа T для добавления.</param>
        void Add(T entity);
        /// <summary>
        /// Обновляет существующий объект типа T в базе данных.
        /// </summary>
        /// <param name="entity">Объект типа T для обновления.</param>
        void Update(T entity);
        /// <summary>
        /// Удаляет объект типа T с указанным идентификатором из базы данных.
        /// </summary>
        /// <param name="id">Идентификатор объекта для удаления.</param>
        void Delete(int id);
    }
}
