using System.Threading.Tasks;

namespace Shopping.Core.UnityOfWork
{
    public interface IUnitOfWork
    {
        Task CommmitAsync();
        void Commit();
    }
}
