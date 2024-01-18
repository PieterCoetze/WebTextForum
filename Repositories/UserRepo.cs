using System.Data.SqlClient;
using System.Data;
using WebTextForum.Models;
using WebTextForum.Repositories.Interfaces;
using Dapper;
using WebTextForum.Models.Dto_s;

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

        public User AuthenticateUser(UserAuthenticateDto userAuthenticateDto)
        {
            var sql = @"
                     SELECT 
	                    [tUsers].[UserId],
	                    [tUsers].[Username],
	                    [tUsers].[Name],
	                    [tUsers].[Surname],
	                    [tUserTypes].[Code] [UserType]
                    FROM 
	                    [tUsers] 
	                    LEFT JOIN [tUserTypes] ON [tUserTypes].[UserTypeId] = [tUsers].[UserTypeId] 
                    WHERE 
	                    [Username] = @username
                    AND [Password] = @password";

            return _sqlConnection.Query<User>(sql, new { userAuthenticateDto.Username, userAuthenticateDto.Password }, transaction: _dbTransaction).FirstOrDefault();
        }

        public List<User> GetUsers(int? userId)
        {
            var sql = @"
                    SELECT 
	                    [tUsers].[UserId],
	                    [tUsers].[Username],
	                    [tUsers].[Name],
	                    [tUsers].[Surname],
	                    [tUserTypes].[Code] [UserType]
                    FROM 
	                    [tUsers] 
	                    LEFT JOIN [tUserTypes] ON [tUserTypes].[UserTypeId] = [tUsers].[UserTypeId]
                    WHERE 
	                    [UserId] = ISNULL(@userId, [UserId])";

            return _sqlConnection.Query<User>(sql, new {userId}, transaction: _dbTransaction).ToList();
        }
    }
}
