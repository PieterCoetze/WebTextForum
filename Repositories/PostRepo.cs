using System.Data.SqlClient;
using System.Data;
using WebTextForum.Repositories.Interfaces;
using WebTextForum.Models;
using Dapper;
using WebTextForum.Models.Dto_s;
using System.Linq.Expressions;
using System.Globalization;

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

        public List<Post> GetPosts(GetPostDto getPostDto)
        {
            var sql = @$"
                   SELECT 
	                [tPosts].[PostId],
	                [tPosts].[PostText],
	                [tPosts].[CreatedDate],
	                [tUsers].[Username] [CreatedBy]
                FROM 
	                [tPosts]
	                LEFT JOIN [tUsers] ON [tUsers].[UserId] = [tPosts].[CreatedBy]
	                LEFT JOIN [tPostFlags] ON [tPostFlags].[PostId] = [tPosts].[PostId]
                WHERE 
	                [tPosts].[PostId] = ISNULL(@postID, [tPosts].[PostId]) 
                    AND [tUsers].[UserId] = ISNULL(@createdBy, [tUsers].[UserId]) 
                    AND [tUsers].[Username] = ISNULL(@createdByUsername, [tUsers].[Username])
                    AND [tPosts].[CreatedDate] >= ISNULL(@startdDate, [tPosts].[CreatedDate])
            ";

            if(getPostDto.PostId == null)
            {
                if (getPostDto.EndDate != null)
                    sql += " AND [tPosts].[CreatedDate] <= @endDate";

                if (getPostDto.Flags != null && getPostDto.Flags.Length > 0)
                    sql += " AND [tPostFlags].[FlagId] IN @flags";

                if (getPostDto.SortBy != null)
                {
                    string sortBy = GetSortByColumn(getPostDto.SortBy);

                    if (sortBy != null)
                        sql += $" ORDER BY {sortBy}";
                }
                else
                    sql += $" ORDER BY [tPosts].[PostId]";

                if (getPostDto.SortDecending)
                    sql += " DESC";

                sql += $" OFFSET {(getPostDto.PageNo - 1) * getPostDto.PageSize} ROWS FETCH NEXT {getPostDto.PageSize} ROWS ONLY";
            }

            return _sqlConnection.Query<Post>(sql, new { getPostDto.PostId, getPostDto.CreatedBy, getPostDto.CreatedByUsername, getPostDto.StartdDate, getPostDto.EndDate, getPostDto.SortBy, getPostDto.Flags }, transaction: _dbTransaction).ToList();
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

        public int AddPost(PostDto postDto, int userId)
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

            return postId;
        }

        private string? GetSortByColumn(string sortBy)
        {
            switch (sortBy)
            {
                case "date":
                    return "CreatedDate";
                case "likeCount":
                    return "(SELECT COUNT(LikeId) FROM [tLikes] WHERE [PostId] = [tPosts].[PostId] AND [IsDeleted] = 0)";
                default:
                    return null;
            }
        }
    }
}
