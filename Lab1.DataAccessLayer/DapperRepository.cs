using Dapper;
using Lab1.DataAccessLayer;
using laba1.Models;
using Microsoft.Data.SqlClient;

public class DapperRepository<T> : IRepository<T> where T : IDomainObject
{
    private readonly string _connectionString;

    public DapperRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    public IEnumerable<T> ReadAll()
    {
        using var db = CreateConnection();
        return db.Query<T>(@"
            SELECT id_bankAccount AS Id,
                   number         AS AccountNumber,
                   id_type        AS TypeId,
                   id_owner       AS OwnerId,
                   date_create    AS DateCreate,
                   balance        AS Balance,
                   isActiv        AS IsActive,
                   isDeleted      AS IsDeleted
            FROM BankAccount");
    }

    public T? ReadById(int id)
    {
        using var db = CreateConnection();
        return db.QueryFirstOrDefault<T>(@"
            SELECT id_bankAccount AS Id, number AS AccountNumber, id_type AS TypeId,
                   id_owner AS OwnerId, date_create AS DateCreate,
                   balance AS Balance, isActiv AS IsActive, isDeleted AS IsDeleted
            FROM BankAccount WHERE id_bankAccount = @Id", new { Id = id });
    }

    public void Add(T entity)
    {
        using var db = CreateConnection();
        db.Execute(@"
            INSERT INTO BankAccount (id_type, id_owner, date_create, balance, isActiv, isDeleted)
            VALUES (@TypeId, @OwnerId, @DateCreate, @Balance, @IsActive, @IsDeleted)",
            entity);
        // ⚠ 'number' не передаём — он генерируется в БД автоматически через DEFAULT/SEQUENCE
    }

    public void Update(T entity)
    {
        using var db = CreateConnection();
        db.Execute(@"
            UPDATE BankAccount
            SET id_owner = @OwnerId, balance = @Balance, isActiv = @IsActive, isDeleted = @IsDeleted
            WHERE id_bankAccount = @Id", entity);
    }

    public void Delete(int id)
    {
        using var db = CreateConnection();
        db.Execute("DELETE FROM BankAccount WHERE id_bankAccount = @Id", new { Id = id });
    }
}