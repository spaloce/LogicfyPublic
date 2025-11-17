using Logicfy.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Logicfy.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveAsync();
    }
}
