
namespace Lab1.Model
{
    /// <summary>
    /// Интерфейс, представляющий доменный объект с уникальным идентификатором.
    /// </summary>
    public interface IDomainObject
    {
        int Id { get; set; }
    }
}