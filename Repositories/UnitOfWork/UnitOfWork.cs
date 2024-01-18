using System.Data;
using WebTextForum.Repositories.Interfaces;

namespace WebTextForum.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepo UserRepo { get; set; }

        IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction, IUserRepo userRepo)
        {
            UserRepo = userRepo;
            _dbTransaction = dbTransaction;
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
