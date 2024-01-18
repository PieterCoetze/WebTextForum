using System.Data;
using WebTextForum.Repositories.Interfaces;

namespace WebTextForum.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepo UserRepo { get; set; }
        public IPostRepo PostRepo { get; set; }
        public ICommentRepo CommentRepo { get; set; }

        IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction, IUserRepo userRepo, IPostRepo postRepo, ICommentRepo commentRepo)
        {
            UserRepo = userRepo;
            _dbTransaction = dbTransaction;
            PostRepo = postRepo;
            CommentRepo = commentRepo;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }
        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
