using Dapper;
using Shopping.Core.Models.SalesDbModels;
using Shopping.Core.Repositories.SalesRespositories;
using Shopping.Core.UnitOfWorks;
using Shopping.Core.UnityOfWork;
using Shopping.Data.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Repositories.Sales
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IUnitOfWorkForDapper unitOfWork;

        private readonly string insert = $@"
                            DECLARE @MyTableVar table(CurrentId uniqueidentifier);

                            INSERT INTO Discounts (StartDate,EndDate,Description,Percentage)
                                        OUTPUT inserted.Id
                                            INTO @MyTableVar
                                            VALUES(@{nameof(Discount.StartDate)},
                                                   @{nameof(Discount.EndDate)},
                                                   @{nameof(Discount.Description)},
                                                   @{nameof(Discount.Percentage)})

                                        SELECT TOP(1) * FROM Discounts WHERE ID IN (SELECT * FROM @MyTableVar)";

        private readonly string update = @$"UPDATE Discounts
                                             SET StartDate = @{nameof(Discount.StartDate)},
                                                 EndDate = @{nameof(Discount.EndDate)},
                                                 Description = @{nameof(Discount.Description)},
                                                 Percentage = @{nameof(Discount.Percentage)}

                                                WHERE Id = @id";

        private readonly string remove = $@"UPDATE Discounts SET DeleteStatus = 1
                                                WHERE Id = @id";

        private readonly string activeDiscountsSql = $@"SELECT * FROM Discounts WHERE DeleteStatus = 0";

        private readonly string discountByIdSql = $@"SELECT * FROM Discounts WHERE Id = @id";

        public DiscountRepository(IUnitOfWorkForDapper unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Discount> CreateAsync(Discount discount)
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Discount>(this.insert, discount, this.unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<IEnumerable<Discount>> GetAllAsync()
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryAsync<Discount>(activeDiscountsSql, null, this.unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async  Task<Discount> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Discount>
                    (discountByIdSql, new { id }, this.unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Remove(Guid id)
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryAsync<bool>(this.remove, new { id }, this.unitOfWork.GetTransaction());
                return result != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<Discount> UpdateAsync(Discount discount)
        {
            try
            {
                await this.unitOfWork.GetConnection().QueryAsync(update, discount, this.unitOfWork.GetTransaction());

                return discount;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
