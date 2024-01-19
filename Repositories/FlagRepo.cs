using System.Data.SqlClient;
using System.Data;
using WebTextForum.Repositories.Interfaces;
using Dapper;
using WebTextForum.Models;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Repositories
{
    public class FlagRepo : IFlagRepo
    {
        private SqlConnection _sqlConnection;
        private IDbTransaction _dbTransaction;

        public FlagRepo(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public List<Flag> GetFlags()
        {
            var sql = @"
                   SELECT
                        [FlagId],
                        [Name],
		                [Code],
		                [Description]
                    FROM 
	                    [tFlags]
                    WHERE 
	                    [IsDeleted] = 0"
            ;

            return _sqlConnection.Query<Flag>(sql, null, transaction: _dbTransaction).ToList();
        }

        public List<PostFlag> GetPostFlags(int postId)
        {
            var sql = @"
                   SELECT
                        [tFlags].[FlagId],
                        [tFlags].[Name],
		                [tFlags].[Code]
                    FROM 
	                    [tPostFlags]
                        LEFT JOIN [tFlags] ON [tFlags].[FlagId] = [tPostFlags].[FlagId]
                        LEFT JOIN [tPosts] ON [tPosts].[PostId] = [tPostFlags].[PostId]
                    WHERE 
                        [tPostFlags].[PostId] = @postId AND
	                    [tPostFlags].[IsDeleted] = 0 AND
                        [tFlags].[IsDeleted] = 0";

            return _sqlConnection.Query<PostFlag>(sql, new { postId }, transaction: _dbTransaction).ToList();
        }

        public bool CheckIfPostHasFlag(int flagId, int postId)
        {
            var sql = @"
                SELECT
                    1
                FROM 
	                [tPostFlags]
                WHERE 
                    [tPostFlags].[PostId] = @postId AND
                    [tPostFlags].[FlagId] = @flagId AND
	                [tPostFlags].[IsDeleted] = 0 
            ";

            return _sqlConnection.Query(sql, new { postId, flagId }, transaction: _dbTransaction).Any();
        }

        public bool PostFlag(PostFlagDto postFlagDto, int userId)
        {
            var sql = @"
                INSERT INTO
	            [tPostFlags]
	            (
		            [PostId],
		            [FlagId],
		            [CreatedBy]
	            )
                VALUES
	            (
		            @postId,
		            @flagId,
                    @userId
	            )";

            var postFlagId = _sqlConnection.Execute(sql, new { postFlagDto.PostId, postFlagDto.FlagId, userId }, transaction: _dbTransaction);

            return postFlagId > 0;
        }
    }
}
