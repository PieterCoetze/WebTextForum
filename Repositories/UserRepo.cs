using System.Data.SqlClient;
using System.Data;
using WebTextForum.Models;
using WebTextForum.Repositories.Interfaces;
using Dapper;

namespace WebTextForum.Repositories
{
    public class UserRepo : IUserRepo
    {
        private SqlConnection _sqlConnection;
        private IDbTransaction _dbTransaction;

        public UserRepo(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public List<User> GetUsers(int? userId)
        {
            var sql = "SELECT * FROM [tUsers] WHERE [UserId] = @UserId";

            return _sqlConnection.Query<User>(sql, new {userId}, transaction: _dbTransaction).ToList();
        }
    }
}
