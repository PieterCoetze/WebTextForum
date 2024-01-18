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

        public int GetPostOwner(int postId)
        {
            var sql = @"
                   SELECT 
                        [CreatedBy]
                    FROM 
	                    [tPosts]
                    WHERE 
	                    [PostId] = @postID";

            return _sqlConnection.QuerySingle<int>(sql, new { postId }, transaction: _dbTransaction);
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
    }
}
