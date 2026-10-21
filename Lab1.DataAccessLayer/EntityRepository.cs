using Lab1.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория для работы с доменными объектами типа T с использованием Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">
    /// Тип доменного объекта, с которым работает репозиторий.
    /// </typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityRepository{T}"/> с указанным контекстом базы данных.
        /// </summary>
        private readonly DbContext _context;
        /// <summary>
        /// Получает набор сущностей типа T.
        /// </summary>
        private readonly DbSet<T> _dbSet;
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityRepository{T}"/> с указанным контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public EntityRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        /// <summary>
        /// Создаёт экземпляр EntityRepository, используя новый экземпляр DBContext.
        /// </summary>
        /// <remarks>Делегирует выполнение другому конструктору, принимающему DBContext.</remarks>
      
        /// <summary>
        /// Добавляет новый объект типа T в базу данных.
        /// </summary>
        /// <param name="item">Объект типа T, который нужно добавить.</param>
        public void Add(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Удаляет объект типа T из базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта типа T, который нужно удалить.</param>
        public void Delete(int id)
        {
            var entity = ReadById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Возвращает все объекты типа T из базы данных.
        /// </summary>
        /// <returns>
        /// Список всех объектов типа T.
        /// </returns>
        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }
        /// <summary>
        /// Возвращает объект типа T с указанным идентификатором из базы данных.
        /// </summary>
        /// <param name="id">Идентификатор объекта типа T, который нужно вернуть.</param>
        /// <returns>
        /// Объект типа T с указанным идентификатором или null, если объект не найден.
        /// </returns>
        public T? ReadById(int id)
        {
            return _dbSet.Find(id);
        }
        /// <summary>
        /// Обновляет существующий объект типа T в базе данных.
        /// </summary>
        /// <param name="item">Объект типа T, который нужно обновить.</param>
        public void Update(T item)
        {
            _dbSet.Update(item);
            _context.SaveChanges();
        }
    }
}
