using Dapper;
using Shopping.Core.Models.SalesDbModels;
using Shopping.Core.Repositories.Sales;
using Shopping.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Repositories.Sales
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IUnitOfWorkForDapper unitOfWork;

        private string insert = $@"INSERT INTO Invoices([CashierId],[CustomerId],[TotalAmount])
                                    OUTPUT inserted.Id
                                    VALUES(@{nameof(Invoice.CashierId)},
                                           @{nameof(Invoice.CustomerId)},
                                           @{nameof(Invoice.TotalAmount)})";

        private string remove = $@"UPDATE Invoices
                                        SET DeleteStatus = 1
                                        Where Id=@id";

        private string update = $@"UPDATE Invoices
                                       SET CustomerId = @{nameof(Invoice.CustomerId)}
                                        WHERE Id = @id";

        private readonly string getByIdSql = $@"SELECT I.Id InvoiceId, I.TotalAmount,I.CreatedDate,
                                                 CA.Name + ' ' + CA.Surname CashierFullName,
                                                 CU.Name + ' ' + CU.Surname CustomerFullName
                                                 FROM INVOICES I
                                                 LEFT JOIN Users CA ON CA.Id = I.CashierId
                                                 LEFT JOIN Users CU ON CU.Id = I.CustomerId
                                                 WHERE I.ID = @id
                                                 SELECT P.Name ProductName, INP.Quantity ProductQuantity,P.Price ProductPrice,
                                                 D.Description DiscountDescription, D.StartDate DiscountStartDate, D.EndDate DiscountEndDate,
                                                 D.Percentage DiscountPercentage
                                                 FROM INVOICES I
                                                 LEFT JOIN INVOICEPRODUCTS INP ON I.ID = INP.INVOICEID
                                                 LEFT JOIN PRODUCTS P ON INP.PRODUCTID = P.ID
                                                 LEFT JOIN Users CA ON CA.Id = I.CashierId
                                                 LEFT JOIN Users CU ON CU.Id = I.CustomerId
                                                 LEFT JOIN Discounts D ON D.Id = P.DiscountId
                                                 WHERE I.ID = @id";

        public InvoiceRepository(IUnitOfWorkForDapper unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async  Task<FullInvoiceDetails> GetByIdAsync(Guid id)
        {
            var response = await this.unitOfWork.GetConnection().QueryMultipleAsync(this.getByIdSql, new { id }, this.unitOfWork.GetTransaction());

            var res = new FullInvoiceDetails
            {
                Invoice = response.ReadFirst<SimpleInvoice>(),
                Products = response.Read<ProductInvoiceItem>()
            };

            return res;
        }

        public async Task<Guid> InsertAsync(Invoice invoice)
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(insert, invoice, this.unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async  Task<bool> RemoveAsync(Guid id)
        {
            try
            {
                var result = await this.unitOfWork.GetConnection().QueryAsync<bool>
                    (this.remove, new { id }, this.unitOfWork.GetTransaction());

                return result != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Invoice> UpdateAsync(Invoice invoice)
        {
            try
            {
                await this.unitOfWork.GetConnection().QueryAsync(update, invoice, this.unitOfWork.GetTransaction());
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
