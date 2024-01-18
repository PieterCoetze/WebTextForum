using System.Data.SqlClient;
using System.Data;
using WebTextForum.Repositories.Interfaces;
using WebTextForum.Models;
using Dapper;
using WebTextForum.Models.Dto_s;

namespace WebTextForum.Repositories
{
    public class PostRepo : IPostRepo
    {
        private SqlConnection _sqlConnection;
        private IDbTransaction _dbTransaction;

        public PostRepo(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public List<Post> GetPosts(int? postId)
        {
            var sql = @"
                   SELECT 
                        [tPosts].[PostId],
	                    [tPosts].[PostText],
                        [tPosts].[CreatedDate],
                        [tUsers].[Username] [CreatedBy]
                    FROM 
	                    [tPosts]
                        LEFT JOIN [tUsers] ON [tUsers].[UserId] = [tPosts].[CreatedBy]
                    WHERE 
	                    [tPosts].[PostId] = ISNULL(@postID, [tPosts].[PostId])";

            return _sqlConnection.Query<Post>(sql, new { postId }, transaction: _dbTransaction).ToList();
        }

        public Post? AddPost(PostDto postDto, int userId)
        {
            var sql = @"
                    INSERT INTO
	                [tPosts]
	                (
		                [PostText],
		                [CreatedBy],
		                [CreatedDate]
	                )
                OUTPUT INSERTED.PostId
                VALUES
	                (
		                @postText,
		                @userId,
		                GETDATE()
	                )";

            var postId = _sqlConnection.QuerySingle<int>(sql, new { postDto.PostText, userId }, transaction: _dbTransaction);

            if (postId > 0)
                return GetPosts(postId).First();

            return null;
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
