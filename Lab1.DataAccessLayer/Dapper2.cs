using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Lab1.Model.ModelsDB;

namespace Lab1.DataAccessLayer
{
    public class DapperBankAccountRepository : IRepository<BankAccount>
    {
        private readonly string _connectionString;

        public DapperBankAccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<BankAccount> ReadAll()
        {
            using (IDbConnection connection =
                   new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT
                        id_bankAccount AS Id,
                        number,
                        id_type,
                        id_owner,
                        date_create,
                        balance,
                        isActiv,
                        isDeleted
                    FROM BankAccount";

                return connection.Query<BankAccount>(sql).AsList();
            }
        }

        public BankAccount? ReadById(int id)
        {
            using (IDbConnection connection =
                   new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT
                        id_bankAccount AS Id,
                        number,
                        id_type,
                        id_owner,
                        date_create,
                        balance,
                        isActiv,
                        isDeleted
                    FROM BankAccount
                    WHERE id_bankAccount = @Id";

                return connection.QueryFirstOrDefault<BankAccount>(
                    sql, new { Id = id });
            }
        }

        public void Add(BankAccount item)
        {
            using (IDbConnection connection =
                   new SqlConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO BankAccount
                    (
                        number,
                        id_type,
                        id_owner,
                        date_create,
                        balance,
                        isActiv,
                        isDeleted
                    )
                    VALUES
                    (
                        @number,
                        @id_type,
                        @id_owner,
                        @date_create,
                        @balance,
                        @isActiv,
                        @isDeleted
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int id = connection.ExecuteScalar<int>(sql, item);

                item.Id = id;
            }
        }

       
        public void Update(BankAccount item)
        {
            using (IDbConnection connection =
                   new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE BankAccount
                    SET
                        number = @number,
                        id_type = @id_type,
                        id_owner = @id_owner,
                        date_create = @date_create,
                        balance = @balance,
                        isActiv = @isActiv,
                        isDeleted = @isDeleted
                    WHERE id_bankAccount = @Id";

                connection.Execute(sql, item);
            }
        }

        
        public void Delete(int id)
        {
            using (IDbConnection connection =
                   new SqlConnection(_connectionString))
            {
                string sql = @"
                    DELETE FROM BankAccount
                    WHERE id_bankAccount = @Id";

                connection.Execute(sql, new { Id = id });
            }
        }
    }
}