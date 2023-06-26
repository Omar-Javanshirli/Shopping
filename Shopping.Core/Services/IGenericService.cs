using SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace Shopping.Core.Services
{
    public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TDto>>> GetAllAsync();

        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

        Task<Response<TDto>> AddAsync(TDto entity);

        Task<Response<NoContent>> Remove(int id);

        Task<Response<NoContent>> Update(TDto entity, int id);
    }
}
