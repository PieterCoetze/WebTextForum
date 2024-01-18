using System.Data;
using WebTextForum.Repositories.Interfaces;

namespace WebTextForum.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepo UserRepo { get; set; }
    }
}
