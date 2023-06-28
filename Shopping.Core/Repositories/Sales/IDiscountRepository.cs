using Shopping.Core.Models.SalesDbModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Shopping.Core.Repositories.SalesRespositories
{
    public interface IDiscountRepository
    {
        Task<Discount> CreateAsync(Discount discount);
        Task<Discount> UpdateAsync(Discount discount);
        Task<bool> Remove(Guid id);
        Task<IEnumerable<Discount>> GetAllAsync();
        Task<Discount> GetByIdAsync(Guid id);
    }
}
