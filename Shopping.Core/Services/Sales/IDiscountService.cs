using SharedLibrary.Dtos;
using Shopping.Core.Models.SalesDbModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Core.Services.Sales
{
    public interface IDiscountService
    {
        Task<Response<Discount>> CreateAsync(Discount discount);
        Task<Response<Discount>> UpdateAsync(Discount discount);
        Task<Response<bool>> Remove(Guid id);
        Task<Response<IEnumerable<Discount>>> GetAllAsync();
        Task<Response<Discount>> GetByIdAsync(Guid id);
    }
}
