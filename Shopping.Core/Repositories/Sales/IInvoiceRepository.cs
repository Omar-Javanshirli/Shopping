using SharedLibrary.Dtos;
using Shopping.Core.Models.SalesDbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Repositories.Sales
{
    public interface IInvoiceRepository
    {
        Task<Guid> InsertAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(Invoice invoice);
        Task<bool> RemoveAsync(Guid id);
        Task<FullInvoiceDetails> GetByIdAsync(Guid id);
    }
}
