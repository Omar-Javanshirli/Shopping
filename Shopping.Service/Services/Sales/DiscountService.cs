using SharedLibrary.Dtos;
using Shopping.Core.Models.SalesDbModels;
using Shopping.Core.Repositories.SalesRespositories;
using Shopping.Core.Services.Sales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Service.Services.Sales
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        public async Task<Response<Discount>> CreateAsync(Discount discount)
        {
            var newDiscount = await discountRepository.CreateAsync(discount);

            if (newDiscount == null)
                return Response<Discount>.Fail("no Content", 404);

            return Response<Discount>.Success(newDiscount, 204);
        }

        public async Task<Response<IEnumerable<Discount>>> GetAllAsync()
        {
            IEnumerable<Discount> discounts = await this.discountRepository.GetAllAsync();

            if (discounts == null)
                return Response<IEnumerable<Discount>>.Fail("no Contents", 404);

            return Response<IEnumerable<Discount>>.Success(discounts, 200);
        }

        public async Task<Response<Discount>> GetByIdAsync(Guid id)
        {
            var discount=await this.discountRepository.GetByIdAsync(id);

            if (discount == null)
                return Response<Discount>.Fail("no Content", 404);

            return Response<Discount>.Success(discount, 200);
        }

        public async Task<Response<bool>> Remove(Guid id)
        {
            var isDiscount = await this.discountRepository.Remove(id);

            if (isDiscount == false)
                return Response<bool>.Fail("no Content", 404);

            return Response<bool>.Success(isDiscount, 200);
        }

        public async Task<Response<Discount>> UpdateAsync(Discount discount)
        {
            var newDiscount = await this.discountRepository.UpdateAsync(discount);

            if (newDiscount == null)
                return Response<Discount>.Fail("no Content", 404);

            return Response<Discount>.Success(newDiscount, 200);
        }
    }
}
