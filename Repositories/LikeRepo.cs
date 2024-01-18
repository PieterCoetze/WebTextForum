using System.Data.SqlClient;
using System.Data;
using WebTextForum.Repositories.Interfaces;
using Dapper;

namespace WebTextForum.Repositories
{
    public class LikeRepo : ILikeRepo
    {
        private SqlConnection _sqlConnection;
        private IDbTransaction _dbTransaction;

        public LikeRepo(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public int GetPostLikeCount(int postId)
        {
            var sql = @"
                SELECT 
                    COUNT([LikeId])
                FROM 
	                [tLikes]
                WHERE 
	                [PostId] = @postID AND
                    [IsDeleted] = 0";

            return _sqlConnection.QuerySingle<int>(sql, new { postId }, transaction: _dbTransaction);
        }

        public bool HasLikedPost(int postId, int userId)
        {
            var sql = @"
                SELECT
	                1
                FROM
	                [tLikes]
                WHERE
	                [PostId] = @postId AND
	                [CreatedBy] = @userId AND
                    [IsDeleted] = 0";

            return _sqlConnection.Query(sql, new { postId, userId }, transaction: _dbTransaction).Any();
        }

        public bool AddLike(int postId, int userId)
        {
            string sql = @"
                INSERT INTO
	            [tLikes]
	            (
                    [PostId],
		            [CreatedBy],
		            [CreatedDate]
	            )
                VALUES
	            (
		            @postId,
		            @userId,
		            GETDATE()
	            )";

            return _sqlConnection.Execute(sql, new { postId, userId }, transaction: _dbTransaction) > 0;
        }

        public bool RemoveLike(int postId, int userId)
        {
            string sql = @"
                UPDATE
	                [tLikes]
                SET
	                [IsDeleted] = 1,
                    [ModifiedDate] = GETDATE()
                WHERE
	                [PostId] = @postId AND
	                [CreatedBy] = @userId";

            return _sqlConnection.Execute(sql, new { postId, userId }, transaction: _dbTransaction) > 0;
        }
    }
}
