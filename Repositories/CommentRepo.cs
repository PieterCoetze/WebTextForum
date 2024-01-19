using System.Data.SqlClient;
using System.Data;
using Dapper;
using WebTextForum.Models.Dto_s;
using WebTextForum.Models;
using WebTextForum.Repositories.Interfaces;

namespace WebTextForum.Repositories
{
    public class CommentRepo : ICommentRepo
    {
        private SqlConnection _sqlConnection;
        private IDbTransaction _dbTransaction;

        public CommentRepo(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public Comment GetComment(int commentId)
        {
            var sql = @"
                   SELECT 
                        [tComments].[CommentId],
                        [tComments].[CommentText],
		                [tComments].[PostId],
		                [tUsers].[Username] [CreatedBy],
		                [tComments].[CreatedDate]
                    FROM 
	                    [tComments]
                        LEFT JOIN [tUsers] ON [tUsers].[UserId] = [tComments].[CreatedBy]
                    WHERE 
	                    [tComments].[CommentId] = @commentId";

            return _sqlConnection.QuerySingle<Comment>(sql, new { commentId }, transaction: _dbTransaction);
        }

        public List<Comment> GetCommentsForPost(int postId)
        {
            var sql = @"
                   SELECT 
                        [tComments].[CommentId],
                        [tComments].[CommentText],
		                [tComments].[PostId],
		                [tUsers].[Username] [CreatedBy],
		                [tComments].[CreatedDate]
                    FROM 
	                    [tComments]
                        LEFT JOIN [tUsers] ON [tUsers].[UserId] = [tComments].[CreatedBy]
                    WHERE 
	                    [tComments].[PostId] = @postId";

            return _sqlConnection.Query<Comment>(sql, new { postId }, transaction: _dbTransaction).ToList();
        }

        public int AddComment(CommentDto commentDto, int userId)
        {
            var sql = @"
                    INSERT INTO
	                [tComments]
	                (
		                [CommentText],
		                [PostId],
		                [CreatedBy],
		                [CreatedDate]
		
	                )
                OUTPUT INSERTED.CommentId
                VALUES
	                (
		                @CommentText,
		                @PostId,
		                @CreatedBy,
		                GETDATE()
	                )"
            ;

           return _sqlConnection.QuerySingle<int>(sql, new { commentDto.CommentText, commentDto.PostId, CreatedBy = userId }, transaction: _dbTransaction);
        }
    }
}
